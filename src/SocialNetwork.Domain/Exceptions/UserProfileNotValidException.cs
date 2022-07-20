namespace SocialNetwork.Domain.Exceptions
{
    public class UserProfileNotValidException : NotValidException
    {
        public UserProfileNotValidException() {}
        public UserProfileNotValidException(string message) : base(message) { }
        public UserProfileNotValidException(string message, Exception inner) : base(message, inner) { }

    }
}
