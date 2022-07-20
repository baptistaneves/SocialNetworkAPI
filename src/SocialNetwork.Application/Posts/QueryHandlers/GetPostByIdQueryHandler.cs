using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Application.Enums;
using SocialNetwork.Application.Models;
using SocialNetwork.Application.Posts.Queries;
using SocialNetwork.Dal.Context;
using SocialNetwork.Domain.Aggregates.PostAggregate;

namespace SocialNetwork.Application.Posts.QueryHandlers
{
    public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, OperationResult<Post>>
    {
        private readonly DataContext _context;

        public GetPostByIdQueryHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<Post>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<Post>();

            try
            {
                var post = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == request.PostId);

                if(post is null)
                {
                    result.IsError = true;
                    result.Errors.Add(new Error
                    {
                        Code = ErrorCode.NotFound,
                        Message = $"No post found with the especified ID {request.PostId}"
                    });

                    return result;
                }

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
