namespace UserSerivceTests.Commands
{
    public sealed class LogoutUserCommandHandlerTests
    {
        [Fact]
        public async Task Handle_should_register_revoked_token_when_token_is_not_revoked_yet()
        {
            // Arrange
            var tokenRepository = new Mock<IRevorkedTokenRepository>(MockBehavior.Strict);
            var command = new LogoutUserCommand("token-id", new DateTime(2026, 6, 29, 10, 30, 0, DateTimeKind.Utc));

            tokenRepository
                .Setup(repository => repository.IsRevorked(command.TokenId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            tokenRepository
                .Setup(repository => repository.RevorkeTokenAsync(command.TokenId, command.ExpiresAtUtc, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var handler = new LogoutUserCommandHandler(tokenRepository.Object);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            tokenRepository.Verify(repository => repository.IsRevorked(command.TokenId, It.IsAny<CancellationToken>()), Times.Once);
            tokenRepository.Verify(repository => repository.RevorkeTokenAsync(command.TokenId, command.ExpiresAtUtc, It.IsAny<CancellationToken>()), Times.Once);
            tokenRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Handle_should_skip_revoke_when_token_is_already_revoked()
        {
            // Arrange
            var tokenRepository = new Mock<IRevorkedTokenRepository>(MockBehavior.Strict);
            var command = new LogoutUserCommand("token-id", new DateTime(2026, 6, 29, 10, 30, 0, DateTimeKind.Utc));

            tokenRepository
                .Setup(repository => repository.IsRevorked(command.TokenId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var handler = new LogoutUserCommandHandler(tokenRepository.Object);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            tokenRepository.Verify(repository => repository.IsRevorked(command.TokenId, It.IsAny<CancellationToken>()), Times.Once);
            tokenRepository.Verify(repository => repository.RevorkeTokenAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()), Times.Never);
            tokenRepository.VerifyNoOtherCalls();
        }
    }
}
