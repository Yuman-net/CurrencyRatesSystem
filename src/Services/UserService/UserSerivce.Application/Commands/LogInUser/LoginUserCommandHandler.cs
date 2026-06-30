using MediatR;
using System.Net.Http.Headers;
using UserService.Application.Abstractions.Repositories;
using UserService.Application.Abstractions.Services;

namespace UserService.Application.Commands.LogInUser
{
    public sealed class LoginUserCommandHandler : IRequestHandler<LogInUserCommand, LoginUserCommandResult>
    {
        private readonly IUserRepository _userRepository;

        private readonly IPasswordHasherService _passwordHasherService;

        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginUserCommandHandler(
            IUserRepository userRepository,
            IPasswordHasherService passwordHasherService,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _passwordHasherService = passwordHasherService;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<LoginUserCommandResult> Handle(LogInUserCommand request, CancellationToken cancellationToken)
        {

            var user = await _userRepository.GetUserByName(request.Name, cancellationToken);

            // exception?
            if (user is null)
            {
                throw new UnauthorizedAccessException($"Invalid login or password");
            }

            var isPasswordVerify = _passwordHasherService.VerifyPassword(request.Password, user.Password);


            if (!isPasswordVerify)
            {
                throw new UnauthorizedAccessException($"Invalid login or password");
            }

            var jwtToken = _jwtTokenGenerator.GenerateJwtToken(user.Id, user.Name);

            return new LoginUserCommandResult(user.Id, user.Name, jwtToken);
        }
    }
}
