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
    public class UpdatePostContentCommandHandler : IRequestHandler<UpdatePostContentCommand, OperationResult<Post>>
    {
        private readonly DataContext _context;

        public UpdatePostContentCommandHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<Post>> Handle(UpdatePostContentCommand request, 
            CancellationToken cancellationToken)
        {
            var result = new OperationResult<Post>();

            try
            {
                var post = await _context.Posts
                    .FirstOrDefaultAsync(p => p.PostId == request.PostId, cancellationToken);

                if (post is null)
                {
                    result.AddError(ErrorCode.NotFound,
                        string.Format(PostsErrorMessages.PostNotFound, request.PostId));
                    return result;
                }

                if(post.UserProfileId != request.UserProfileId)
                {
                    result.AddError(ErrorCode.PostUpdateNotPossible, PostsErrorMessages.PostUpdateNotPossible);
                    return result;
                }

                post.UpdatePostText(request.NewText);

                _context.Posts.Update(post);
                await _context.SaveChangesAsync(cancellationToken);

                result.Payload = post;
            }

            catch (PostNotValidException ex)
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
