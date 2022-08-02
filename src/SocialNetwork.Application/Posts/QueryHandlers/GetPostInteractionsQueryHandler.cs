using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Application.Enums;
using SocialNetwork.Application.Models;
using SocialNetwork.Application.Posts.Queries;
using SocialNetwork.Dal.Context;
using SocialNetwork.Domain.Aggregates.PostAggregate;

namespace SocialNetwork.Application.Posts.QueryHandlers
{
    public class GetPostInteractionsQueryHandler : IRequestHandler<GetPostInteractionsQuery, 
        OperationResult<List<PostInteraction>>>
    {
        private readonly DataContext _context;

        public GetPostInteractionsQueryHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<List<PostInteraction>>> Handle(GetPostInteractionsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<List<PostInteraction>>();

            try
            {
                var post = await _context.Posts
                    .Include(p=>p.Interactions)
                    .Include(p=> p.UserProfile)
                    .Where(p=>p.PostId == request.PostId)
                    .FirstOrDefaultAsync(p=> p.PostId == request.PostId, cancellationToken);

                if (post == null)
                {
                    result.AddError(ErrorCode.NotFound, PostsErrorMessages.PostNotFound);
                    return result;
                }

                result.Payload = post.Interactions.ToList();
            }
            catch (Exception ex)
            {
                result.AddUnknownError($"{ex.Message}");
            }

            return result;
        }
    }
}
