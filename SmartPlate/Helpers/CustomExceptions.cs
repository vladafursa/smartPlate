namespace SmartPlate.Helpers
{
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException() : base("User with this email or username already exists.") { }
    }

    public class UserNotFoundException : Exception
    {
        public UserNotFoundException() : base("User not found.") { }
    }

    public class InvalidPasswordException : Exception
    {
        public InvalidPasswordException() : base("Invalid password.") { }
    }
}

