using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Application.Enums;
using SocialNetwork.Application.Models;
using SocialNetwork.Application.Posts.Commands;
using SocialNetwork.Dal.Context;
using SocialNetwork.Domain.Aggregates.PostAggregate;

namespace SocialNetwork.Application.Posts.CommandHandlers
{
    public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, OperationResult<Post>>
    {
        private readonly DataContext _context;

        public DeletePostCommandHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<Post>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<Post>();

            try
            {
                var post = await _context.Posts
                .FirstOrDefaultAsync(up => up.PostId == request.PostId, cancellationToken);


                if (post is null)
                {
                    result.AddError(ErrorCode.NotFound,
                        string.Format(PostsErrorMessages.PostNotFound, request.PostId));
                    return result;
                }

                if (post.UserProfileId != request.UserProfileId)
                {
                    result.AddError(ErrorCode.PostDeleteNotPossible, PostsErrorMessages.PostDeleteNotPossible);
                    return result;
                }

                _context.Posts.Remove(post);

                await _context.SaveChangesAsync(cancellationToken);

                result.Payload = post;

            }
            catch (Exception ex)
            {
                result.AddUnknownError($"{ex.Message}");
            }

            return result;
        }
    }
}
