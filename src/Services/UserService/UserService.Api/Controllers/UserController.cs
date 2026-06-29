using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UserSerivce.Application.Commands.RegisterUser;
using UserService.Api.Contracts.InModels;
using UserService.Api.Contracts.OutModels;
using UserService.Api.Models.InModels;
using UserService.Application.Commands.LogInUser;
using UserService.Application.Commands.LogoutUser;

namespace UserService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class UserController : ControllerBase
    {
        private readonly ISender _sender;

        public UserController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Guid>> Register([FromBody] RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var command = new RegisterUserCommand(request.Name, request.Password);

            var result = await _sender.Send(command, cancellationToken);

            return Ok(result);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LoginUserResponse>> Login([FromBody] LoginUserRequest request, CancellationToken cancellationToken)
        {
            var command = new LogInUserCommand(request.Name, request.Password);

            var userInfo = await _sender.Send(command, cancellationToken);

            var result = new LoginUserResponse(userInfo.Id, userInfo.Name, userInfo.Token);

            return Ok(result);
        }

        [Authorize]
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Logout(CancellationToken cancellationToken)
        {
            var tokenId = User.FindFirstValue(JwtRegisteredClaimNames.Jti);
            var exp = User.FindFirstValue(JwtRegisteredClaimNames.Exp);
            
            if (tokenId is null || exp is null || !long.TryParse(exp, out var expiresAtUnix))
            {
                return Unauthorized();
            }

            var expiresAtUtc = DateTimeOffset
                .FromUnixTimeSeconds(expiresAtUnix)
                .UtcDateTime;

            var command = new LogoutUserCommand(tokenId, expiresAtUtc);

            await _sender.Send(command, cancellationToken);

            return NoContent();
        }
    }
}
