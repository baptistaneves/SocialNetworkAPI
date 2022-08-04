using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Application.Enums;
using SocialNetwork.Application.Models;
using SocialNetwork.Application.Posts.Commands;
using SocialNetwork.Dal.Context;
using SocialNetwork.Domain.Aggregates.PostAggregate;

namespace SocialNetwork.Application.Posts.CommandHandlers
{
    public class UpdatePostCommentCommandHandler : IRequestHandler<UpdatePostCommentCommand, OperationResult<PostComment>>
    {
        private readonly DataContext _context;
        private readonly OperationResult<PostComment> _result;
        public UpdatePostCommentCommandHandler(DataContext context)
        {
            _context = context;
            _result = new OperationResult<PostComment>();
        }

        public async Task<OperationResult<PostComment>> Handle(UpdatePostCommentCommand request, 
            CancellationToken cancellationToken)
        {
            try
            {
                var post = await _context.Posts
                    .Include(p => p.Comments)
                    .FirstOrDefaultAsync(p => p.PostId == request.PostId, cancellationToken);

                if (post == null)
                {
                    _result.AddError(ErrorCode.NotFound,
                        string.Format(PostsErrorMessages.PostNotFound, request.PostId));
                    return _result;
                }

                var comment = post.Comments.FirstOrDefault(c => c.CommentId == request.CommentId);
                if (comment == null)
                {
                    _result.AddError(ErrorCode.NotFound, PostsErrorMessages.PostCommentNotFound);
                    return _result;
                }

                if (comment.UserProfileId != request.UserProfileId)
                {
                    _result.AddError(ErrorCode.CommentUpdateNotAuthorized,
                       PostsErrorMessages.CommentUpdateNotAuthorized);
                    return _result;
                }

                comment.UpdateCommentText(request.UpdatedText);
                _context.Posts.Update(post);
                await _context.SaveChangesAsync(cancellationToken);

                _result.Payload = comment;
            }
            catch (Exception ex)
            {
                _result.AddUnknownError($"{ex.Message}");
            }

            return _result;
        }
    }
}
