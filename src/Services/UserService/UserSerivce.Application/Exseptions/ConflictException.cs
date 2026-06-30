namespace UserService.Application.Exseptions
{
    public sealed class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException(string message)
            : base($"User with name '{message}' already exists.")
        {
        }
    }
}
