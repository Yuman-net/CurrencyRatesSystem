using MediatR;
using UserService.Application.Abstractions.Repositories;
using UserService.Application.Abstractions.Services;
using UserService.Application.Exseptions;
using UserService.Domain;

namespace UserSerivce.Application.Commands.RegisterUser
{
    public sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;

        private readonly IPasswordHasherService _passwordHasher;

        public RegisterUserCommandHandler(IUserRepository userRepository, IPasswordHasherService passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var existUser = await _userRepository.IsExistsAsync(request.Name, cancellationToken);

            if (existUser)
            {
                throw new UserAlreadyExistsException(request.Name);
            }

            var passwordHash = _passwordHasher.HashPassword(request.Password);

            var userEntity = User.Create(request.Name, passwordHash);

            var newUserId = await _userRepository.AddAsync(userEntity, cancellationToken);


            return newUserId;
        }
    }
}
