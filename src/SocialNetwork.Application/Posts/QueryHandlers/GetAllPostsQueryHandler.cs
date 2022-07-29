using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Application.Enums;
using SocialNetwork.Application.Models;
using SocialNetwork.Application.Posts.Queries;
using SocialNetwork.Dal.Context;
using SocialNetwork.Domain.Aggregates.PostAggregate;

namespace SocialNetwork.Application.Posts.QueryHandlers
{
    public class GetAllPostsQueryHandler : IRequestHandler<GetAllPostsQuery, OperationResult<List<Post>>>
    {
        private readonly DataContext _context;

        public GetAllPostsQueryHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<List<Post>>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<List<Post>>();

            try 
            {
                result.Payload = await _context.Posts.ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                result.AddUnknownError($"{ex.Message}");
            }

            return result;
        }
    }
}
