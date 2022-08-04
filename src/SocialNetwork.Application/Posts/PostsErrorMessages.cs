namespace SocialNetwork.Application.Posts
{
    internal class PostsErrorMessages
    {
        public const string PostNotFound = "No Post found with the especified ID {0}";
        public const string PostDeleteNotPossible = "Post delete not possible because it's not the post owner that initiates the update";
        public const string PostUpdateNotPossible = "Post update not possible because it's not the post owner that initiates the update";
        public const string PostInteractionNotFound = "Interaction not found";
        public const string PostCommentNotFound = "Comment not found";
        public const string InteractionRemovalNotAuthorized = "Cannot remove interaction as you not its author";
        public const string CommentRemovalNotAuthorized = "Cannot remove comment as you not its author";
        public const string CommentUpdateNotAuthorized = "Cannot update comment as you not its author";
    }
}
