using SocialNetwork.Domain.Aggregates.UserProfileAggregate;

namespace SocialNetwork.Domain.Aggregates.PostAggregate
{
    public class PostInteraction
    {
        private PostInteraction()
        {
        }

        public Guid InteractionId { get; private set; }
        public Guid PostId { get; private set; }
        public Guid? UserProfileId { get; private set; }
        public UserProfile UserProfile { get; private set; }
        public InteractionType Interaction { get; private set; }

        public static PostInteraction CreatePostInteraction(Guid postId, Guid userProfileId, InteractionType type)
        {
            return new PostInteraction
            {
                PostId = postId,
                Interaction = type,
                UserProfileId = userProfileId
            };
        }

    }
}
