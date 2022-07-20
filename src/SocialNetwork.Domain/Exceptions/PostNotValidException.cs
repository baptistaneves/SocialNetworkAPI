namespace SocialNetwork.Domain.Exceptions
{
    public class PostNotValidException : NotValidException
    {
        public PostNotValidException() {}
        public PostNotValidException(string message) : base(message) {}
        public PostNotValidException(string message, Exception inner) : base(message, inner) { }
    }
}
