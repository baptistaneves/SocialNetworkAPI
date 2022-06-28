namespace SocialNetwork.Domain.Aggregates.PostAggregate
{
    public class PostInteraction
    {
        private PostInteraction()
        {
        }

        public Guid InteractionId { get; private set; }
        public Guid PostId { get; private set; }
        public InteractionType Interaction { get; private set; }

        public static PostInteraction CreatePostInteraction(Guid postId, InteractionType type)
        {
            return new PostInteraction
            {
                PostId = postId,
                Interaction = type
            };
        }

    }
}
