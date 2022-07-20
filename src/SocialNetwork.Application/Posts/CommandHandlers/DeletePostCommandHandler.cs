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
                .FirstOrDefaultAsync(up => up.PostId == request.PostId);


                if (post is null)
                {
                    result.IsError = true;
                    result.Errors.Add(new Error
                    {
                        Code = ErrorCode.NotFound,
                        Message = $"No Post found with the especified ID {request.PostId}"
                    });
                    return result;
                }

                _context.Posts.Remove(post);

                await _context.SaveChangesAsync();

                result.Payload = post;

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
