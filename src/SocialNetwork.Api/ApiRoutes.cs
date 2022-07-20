namespace SocialNetwork.Api
{
    public class ApiRoutes
    {
        public const string BaseRoute = "api/v{version:apiVersion}/[controller]";

        public class UserProfile
        {
            public const string IdRoute = "{id}";

        }

        public class Post
        {
            public const string IdRoute = "{id}";
            public const string PostComments = "{postId}/comments";
            public const string CommentById = "{postId}/comments/{commentId}";
        }
    }
}
