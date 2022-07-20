using SocialNetwork.Domain.Aggregates.UserProfileAggregate;
using SocialNetwork.Domain.Exceptions;
using SocialNetwork.Domain.Validators.PostValidators;

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

        /// <summary>
        /// Creates a new post instance
        /// </summary>
        /// <param name="userProfileId">User profile ID </param>
        /// <param name="textContent">Post content</param>
        /// <returns see cref="Post"></returns>
        /// <exception cref="PostNotValidException"></exception>
        public static Post CreatePost(Guid userProfileId, string textContent)
        {
            var validator = new PostValidator();

            var objToValidate = new Post
            {
                UserProfileId = userProfileId,
                TextContent = textContent,
                CreatedAt = DateTime.UtcNow,
                LastModified = DateTime.UtcNow,
            };

            var validationResult = validator.Validate(objToValidate);

            if (validationResult.IsValid) return objToValidate;

            var exception = new PostNotValidException("Post is not valid");

            validationResult.Errors.ForEach(vr => exception.ValidationErrors.Add(vr.ErrorMessage));

            throw exception;
        }

        /// <summary>
        /// Updates the post text
        /// </summary>
        /// <param name="newText">The updated post text</param>
        /// <exception cref="PostNotValidException"></exception>
        public void UpdatePostText(string newText)
        {
            if(string.IsNullOrWhiteSpace(newText))
            {
                var exception = new PostNotValidException("Can not update post. Post text is not valid");

                exception.ValidationErrors.Add("The provided text is either null or contains only white space");

                throw exception;
            }

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
