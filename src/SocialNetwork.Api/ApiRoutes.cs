namespace SocialNetwork.Api
{
    public static class ApiRoutes
    {
        public const string BaseRoute = "api/v{version:apiVersion}/[controller]";

        public static class UserProfile
        {
            public const string IdRoute = "{id}";

        }

        public static class Post
        {
            public const string IdRoute = "{id}";
            public const string PostComments = "{postId}/comments";
            public const string CommentById = "{postId}/comments/{commentId}";
            public const string InteractionById = "{postId}/interaction/{interactionId}";
            public const string PostInteractions = "{postId}/interactions";
        }

        public static class Identity
        {
            public const string Login = "login";
            public const string Resgistration = "registration";
        }
    }
}
