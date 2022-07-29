using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Application.Enums;
using SocialNetwork.Application.Models;
using SocialNetwork.Application.Posts.Commands;
using SocialNetwork.Dal.Context;
using SocialNetwork.Domain.Aggregates.PostAggregate;
using SocialNetwork.Domain.Exceptions;

namespace SocialNetwork.Application.Posts.CommandHandlers
{
    public class AddPostCommentCommandHandler : IRequestHandler<AddPostCommentCommand, OperationResult<PostComment>>
    {
        private readonly DataContext _context;

        public AddPostCommentCommandHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<PostComment>> Handle(AddPostCommentCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PostComment>();

            try
            {
                var post = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == request.PostId, 
                    cancellationToken);

                if (post is null)
                {
                    result.AddError(ErrorCode.NotFound, 
                        string.Format(PostsErrorMessages.PostNotFound, request.PostId));
                    return result;
                }

                var comment = PostComment.CreatePostComment(request.PostId, request.CommentText, request.UserProfileId);

                post.AddPostComment(comment);

                _context.Posts.Update(post);
                await _context.SaveChangesAsync(cancellationToken);

                result.Payload = comment;
            }
            catch (PostCommentNotValidException ex)
            {
                ex.ValidationErrors.ForEach(error => result.AddError(ErrorCode.ValidationError, $"{error}"));
            }

            catch (Exception ex)
            {
                result.AddUnknownError($"{ex.Message}");
            }

            return result;
        }
    }
}
