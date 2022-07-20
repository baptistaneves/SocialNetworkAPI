namespace SocialNetwork.Domain.Exceptions
{
    public class PostCommentNotValidException : NotValidException
    {
        public PostCommentNotValidException() {}

        public PostCommentNotValidException(string message) : base(message) {}

        public PostCommentNotValidException(string message, Exception inner) : base(message, inner) {}
    }
}
