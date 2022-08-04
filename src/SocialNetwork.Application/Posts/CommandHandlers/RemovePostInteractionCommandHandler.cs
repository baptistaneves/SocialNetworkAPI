using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Application.Enums;
using SocialNetwork.Application.Models;
using SocialNetwork.Application.Posts.Commands;
using SocialNetwork.Dal.Context;
using SocialNetwork.Domain.Aggregates.PostAggregate;

namespace SocialNetwork.Application.Posts.CommandHandlers
{
    public class RemovePostInteractionCommandHandler : IRequestHandler<RemovePostInteractionCommand, 
        OperationResult<PostInteraction>>
    {
        private readonly DataContext _context;

        public RemovePostInteractionCommandHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<PostInteraction>> Handle(RemovePostInteractionCommand request,
            CancellationToken cancellationToken)
        {
            var result = new OperationResult<PostInteraction>();

            try
            {
                var post = await _context.Posts.Include(p => p.Interactions)
                    .FirstOrDefaultAsync(p => p.PostId == request.PostId, cancellationToken);

                if (post == null)
                {
                    result.AddError(ErrorCode.NotFound, 
                        string.Format(PostsErrorMessages.PostNotFound, request.PostId));
                    return result;
                }

                var interaction = post.Interactions.FirstOrDefault(i => i.InteractionId == request.InteractionId);

                if(interaction == null)
                {
                    result.AddError(ErrorCode.NotFound, PostsErrorMessages.PostInteractionNotFound);
                    return result;
                }

                if(interaction.UserProfileId != request.UserProfileId)
                {
                    result.AddError(ErrorCode.InteractionRemovalNotAuthorized, 
                        PostsErrorMessages.InteractionRemovalNotAuthorized);
                    return result;
                }

                post.RemovePostInteraction(interaction);

                _context.Posts.Update(post);
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
