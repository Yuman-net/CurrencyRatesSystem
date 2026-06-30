namespace UserSerivceTests.Domain
{
    public sealed class UserTests
    {
        public static TheoryData<string?, string?> InvalidCreateData => new()
        {
            { null, "hashed-password" },
            { string.Empty, "hashed-password" },
            { "   ", "hashed-password" },
            { "alice", null },
            { "alice", string.Empty },
            { "alice", "   " }
        };

        [Theory]
        [MemberData(nameof(InvalidCreateData))]
        public void Create_should_throw_when_name_or_password_is_invalid(string? name, string? hashPassword)
        {
            Assert.Throws<ArgumentException>(() => User.Create(name!, hashPassword!));
        }

        [Fact]
        public void Create_should_return_user_with_expected_values()
        {
            var user = User.Create("alice", "hashed-password");

            Assert.Equal("alice", user.Name);
            Assert.Equal("hashed-password", user.Password);
            Assert.NotEqual(Guid.Empty, user.Id);
        }
    }
}
