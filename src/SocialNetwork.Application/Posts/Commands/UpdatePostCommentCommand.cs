using MediatR;
using SocialNetwork.Application.Models;
using SocialNetwork.Domain.Aggregates.PostAggregate;

namespace SocialNetwork.Application.Posts.Commands
{
    public class UpdatePostCommentCommand : IRequest<OperationResult<PostComment>>
    {
        public Guid PostId { get; set; }
        public Guid UserProfileId { get; set; }
        public Guid CommentId { get; set; }
        public string UpdatedText { get; set; }
    }
}
