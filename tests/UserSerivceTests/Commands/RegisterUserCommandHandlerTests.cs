namespace UserSerivceTests.Commands
{
    public sealed class RegisterUserCommandHandlerTests
    {
        [Fact]
        public async Task Handle_should_create_user_and_return_created_id_when_name_is_free()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
            var passwordHasher = new Mock<IPasswordHasherService>(MockBehavior.Strict);
            var command = new RegisterUserCommand("alice", "secret-password");
            var expectedId = Guid.NewGuid();
            User? capturedUser = null;

            userRepository
                .Setup(repository => repository.IsExistsAsync(command.Name, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            passwordHasher
                .Setup(hasher => hasher.HashPassword(command.Password))
                .Returns("hashed-password");

            userRepository
                .Setup(repository => repository.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Callback<User, CancellationToken>((user, _) => capturedUser = user)
                .ReturnsAsync(expectedId);

            var handler = new RegisterUserCommandHandler(userRepository.Object, passwordHasher.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(capturedUser);
            Assert.Equal(expectedId, result);
            Assert.Equal(command.Name, capturedUser!.Name);
            Assert.Equal("hashed-password", capturedUser.Password);

            userRepository.Verify(repository => repository.IsExistsAsync(command.Name, It.IsAny<CancellationToken>()), Times.Once);
            passwordHasher.Verify(hasher => hasher.HashPassword(command.Password), Times.Once);
            userRepository.Verify(repository => repository.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
            userRepository.VerifyNoOtherCalls();
            passwordHasher.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Handle_should_throw_when_user_already_exists()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
            var passwordHasher = new Mock<IPasswordHasherService>(MockBehavior.Strict);
            var command = new RegisterUserCommand("alice", "secret-password");

            userRepository
                .Setup(repository => repository.IsExistsAsync(command.Name, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var handler = new RegisterUserCommandHandler(userRepository.Object, passwordHasher.Object);

            // Act
            var exception = await Assert.ThrowsAsync<UserAlreadyExistsException>(
                () => handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.Contains(command.Name, exception.Message);

            userRepository.Verify(repository => repository.IsExistsAsync(command.Name, It.IsAny<CancellationToken>()), Times.Once);
            passwordHasher.VerifyNoOtherCalls();
            userRepository.VerifyNoOtherCalls();
        }
    }
}
