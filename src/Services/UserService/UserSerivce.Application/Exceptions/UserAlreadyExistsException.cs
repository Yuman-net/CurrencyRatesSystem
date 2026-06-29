namespace UserService.Application.Exceptions
{
    public sealed class UserAlreadyExistsException : InvalidOperationException
    {
        public UserAlreadyExistsException(string userName)
            : base($"User with name '{userName}' already exists")
        {
        }
    }
}