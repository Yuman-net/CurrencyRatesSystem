namespace UserSerivceTests.Commands
{
    public sealed class LoginUserCommandHandlerTests
    {
        [Fact]
        public async Task Handle_should_return_token_when_credentials_are_valid()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
            var passwordHasher = new Mock<IPasswordHasherService>(MockBehavior.Strict);
            var tokenGenerator = new Mock<IJwtTokenGenerator>(MockBehavior.Strict);
            var command = new LogInUserCommand("alice", "secret-password");
            var user = User.Create(command.Name, "hashed-password");
            var accessToken = "jwt-token";

            userRepository
                .Setup(repository => repository.GetUserByName(command.Name, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            passwordHasher
                .Setup(hasher => hasher.VerifyPassword(command.Password, user.Password))
                .Returns(true);

            tokenGenerator
                .Setup(generator => generator.GenerateJwtToken(user.Id, user.Name))
                .Returns(accessToken);

            var handler = new LoginUserCommandHandler(userRepository.Object, passwordHasher.Object, tokenGenerator.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(user.Id, result.Id);
            Assert.Equal(user.Name, result.Name);
            Assert.Equal(accessToken, result.Token);

            userRepository.Verify(repository => repository.GetUserByName(command.Name, It.IsAny<CancellationToken>()), Times.Once);
            passwordHasher.Verify(hasher => hasher.VerifyPassword(command.Password, user.Password), Times.Once);
            tokenGenerator.Verify(generator => generator.GenerateJwtToken(user.Id, user.Name), Times.Once);
            userRepository.VerifyNoOtherCalls();
            passwordHasher.VerifyNoOtherCalls();
            tokenGenerator.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Handle_should_throw_when_user_is_missing()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
            var passwordHasher = new Mock<IPasswordHasherService>(MockBehavior.Strict);
            var tokenGenerator = new Mock<IJwtTokenGenerator>(MockBehavior.Strict);
            var command = new LogInUserCommand("alice", "secret-password");

            userRepository
                .Setup(repository => repository.GetUserByName(command.Name, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            var handler = new LoginUserCommandHandler(userRepository.Object, passwordHasher.Object, tokenGenerator.Object);

            // Act
            await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => handler.Handle(command, CancellationToken.None));

            // Assert
            userRepository.Verify(repository => repository.GetUserByName(command.Name, It.IsAny<CancellationToken>()), Times.Once);
            passwordHasher.VerifyNoOtherCalls();
            tokenGenerator.VerifyNoOtherCalls();
            userRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Handle_should_throw_when_password_is_invalid()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
            var passwordHasher = new Mock<IPasswordHasherService>(MockBehavior.Strict);
            var tokenGenerator = new Mock<IJwtTokenGenerator>(MockBehavior.Strict);
            var command = new LogInUserCommand("alice", "wrong-password");
            var user = User.Create(command.Name, "hashed-password");

            userRepository
                .Setup(repository => repository.GetUserByName(command.Name, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            passwordHasher
                .Setup(hasher => hasher.VerifyPassword(command.Password, user.Password))
                .Returns(false);

            var handler = new LoginUserCommandHandler(userRepository.Object, passwordHasher.Object, tokenGenerator.Object);

            // Act
            await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => handler.Handle(command, CancellationToken.None));

            // Assert
            userRepository.Verify(repository => repository.GetUserByName(command.Name, It.IsAny<CancellationToken>()), Times.Once);
            passwordHasher.Verify(hasher => hasher.VerifyPassword(command.Password, user.Password), Times.Once);
            tokenGenerator.VerifyNoOtherCalls();
            userRepository.VerifyNoOtherCalls();
            passwordHasher.VerifyNoOtherCalls();
        }
    }
}
