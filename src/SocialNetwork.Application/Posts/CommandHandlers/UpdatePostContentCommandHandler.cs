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
                var post = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == request.PostId);

                if (post is null)
                {
                    result.IsError = true;
                    var error = new Error { Code = ErrorCode.NotFound, 
                        Message = $"No Post found with ID {request.PostId}" };
                    result.Errors.Add(error);

                    return result;
                }

                if(post.UserProfileId != request.UserProfileId)
                {
                    result.IsError = true;
                    var error = new Error
                    {
                        Code = ErrorCode.PostUpdateNotPossible,
                        Message = $"Post update not possible because it's not the post owner that initiates the update"
                    };
                    result.Errors.Add(error);

                    return result;
                }

                post.UpdatePostText(request.NewText);

                _context.Posts.Update(post);
                await _context.SaveChangesAsync();

                result.Payload = post;

            }

            catch (PostNotValidException ex)
            {

                result.IsError = true;
                ex.ValidationErrors.ForEach(error =>
                {
                    result.Errors.Add(new Error { Code = ErrorCode.ValidationError, Message = $"{error}" });
                });
            }

            catch (Exception ex)
            {

                result.IsError = true;
                result.Errors.Add(new Error { Code = ErrorCode.UnknownError, Message = ex.Message });
            }

            return result;
        }
    }
}
