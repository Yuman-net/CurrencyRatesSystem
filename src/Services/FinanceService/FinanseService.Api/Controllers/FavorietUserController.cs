using FinanceService.Application.Queries;
using FinanceService.Application.QueryDtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinanseService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FinanceController : ControllerBase
    {
        private readonly ISender _sender;

        public FinanceController(ISender sender)
        {
            _sender = sender;
        }

        [Authorize]
        [HttpGet("currencies")]
        [ProducesResponseType(typeof(IReadOnlyCollection<FavoritesUserCurrencyDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IReadOnlyCollection<FavoritesUserCurrencyDto>>> GetFavorietsByUserId(
            CancellationToken cancellationToken)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdValue, out var userId))
            {
                return Unauthorized();
            }

            var request = new CurrencyByUserQuery(userId);

            var result = await _sender.Send(request, cancellationToken);

            return Ok(result);
        }
    }
}
