using SocialNetwork.Domain.Aggregates.UserProfileAggregate;

namespace SocialNetwork.Domain.Aggregates.PostAggregate
{
    public class Post
    {
        private readonly List<PostComment> _commentes = new List<PostComment>();
        private readonly List<PostInteraction> _interactions = new List<PostInteraction>();
        private Post()
        {
        }

        public Guid PostId { get; private set; }
        public Guid UserProfileId { get; private set; }
        public UserProfile UserProfile { get; private set; }
        public string TextContent { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime LastModified { get; private set; }
        public IEnumerable<PostComment> Comments { get { return _commentes; } }
        public IEnumerable<PostInteraction> Interactions { get { return _interactions; } }

        public static Post CreatePost(Guid userProfileId, string textContent)
        {
            return new Post
            {
                UserProfileId = userProfileId,
                TextContent = textContent,
                CreatedAt = DateTime.UtcNow,
                LastModified = DateTime.UtcNow,
            };
        }

        public void UpdatePostText(string newText)
        {
            TextContent = newText;
            LastModified = DateTime.UtcNow;
        }

        public void AddPostComment(PostComment newComment)
        {
            _commentes.Add(newComment);
        }

        public void RemovePostComment(PostComment toRemove)
        {
            _commentes.Remove(toRemove);
        }

        public void AddPostInteraction(PostInteraction newIteraction)
        {
            _interactions.Add(newIteraction);
        }

        public void RemovePostInteraction(PostInteraction toRemove)
        {
            _interactions.Remove(toRemove);
        }
    }
}
