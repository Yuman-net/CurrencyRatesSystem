namespace UserService.Domain
{
    public sealed class User
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string Password { get; private set; }

        private User(Guid id, string name, string password)
        {
            Id = id;
            Name = name;
            Password = password;
        }

        public static User Create(string name, string hashPassword)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"Field {nameof(name)} is required");

            if (string.IsNullOrWhiteSpace(hashPassword))
                throw new ArgumentException($"Field {nameof(hashPassword)} is required");

            return new User(Guid.NewGuid(), name, hashPassword);
        }
    }
}