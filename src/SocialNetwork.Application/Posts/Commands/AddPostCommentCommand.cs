using MediatR;
using SocialNetwork.Application.Models;
using SocialNetwork.Domain.Aggregates.PostAggregate;

namespace SocialNetwork.Application.Posts.Commands
{
    public class AddPostCommentCommand : IRequest<OperationResult<PostComment>>
    {
        public Guid PostId { get; set; }
        public Guid UserProfileId { get; set; }
        public string CommentText { get; set; }
    }
}
