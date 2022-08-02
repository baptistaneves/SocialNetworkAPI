using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Application.Enums;
using SocialNetwork.Application.Models;
using SocialNetwork.Application.Posts.Commands;
using SocialNetwork.Dal.Context;
using SocialNetwork.Domain.Aggregates.PostAggregate;

namespace SocialNetwork.Application.Posts.CommandHandlers
{
    public class AddInteractionCommandHandler : IRequestHandler<AddInteractionCommand, 
        OperationResult<PostInteraction>>
    {
        private readonly DataContext _context;

        public AddInteractionCommandHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<PostInteraction>> Handle(AddInteractionCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PostInteraction>();

            try
            {
                var post = await _context.Posts.Include(p => p.Interactions)
                    .FirstOrDefaultAsync(p => p.PostId == request.PostId, cancellationToken);

                if (post == null)
                {
                    result.AddError(ErrorCode.NotFound, PostsErrorMessages.PostNotFound);
                    return result;
                }

                var interaction = PostInteraction.CreatePostInteraction(request.PostId, request.UserProfileId,
                    request.Type);

                post.AddPostInteraction(interaction);

                _context.Update(post);

                await _context.SaveChangesAsync(cancellationToken);

                result.Payload = interaction;
            }
            catch (Exception ex)
            {
                result.AddUnknownError($"{ex.Message}");
            }

            return result;
        }
    }
}
