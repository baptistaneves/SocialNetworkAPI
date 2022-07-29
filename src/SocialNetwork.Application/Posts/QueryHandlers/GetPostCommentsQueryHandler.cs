using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Application.Enums;
using SocialNetwork.Application.Models;
using SocialNetwork.Application.Posts.Queries;
using SocialNetwork.Dal.Context;
using SocialNetwork.Domain.Aggregates.PostAggregate;

namespace SocialNetwork.Application.Posts.QueryHandlers
{
    public class GetPostCommentsQueryHandler : IRequestHandler<GetPostCommentsQuery, OperationResult<List<PostComment>>>
    {
        private readonly DataContext _context;

        public GetPostCommentsQueryHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<List<PostComment>>> Handle(GetPostCommentsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<List<PostComment>>();

            try
            {
                var post = await _context.Posts
                                .Include(p => p.Comments)
                                .FirstOrDefaultAsync(p => p.PostId == request.PostId, cancellationToken);

                result.Payload = post.Comments.ToList();
            }
            catch (Exception ex)
            {
                result.AddUnknownError($"{ex.Message}");
            }

            return result;
        }
    }
}
