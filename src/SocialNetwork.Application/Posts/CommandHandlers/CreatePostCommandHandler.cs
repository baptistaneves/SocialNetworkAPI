using MediatR;
using SocialNetwork.Application.Enums;
using SocialNetwork.Application.Models;
using SocialNetwork.Application.Posts.Commands;
using SocialNetwork.Dal.Context;
using SocialNetwork.Domain.Aggregates.PostAggregate;
using SocialNetwork.Domain.Exceptions;

namespace SocialNetwork.Application.Posts.CommandHandlers
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, OperationResult<Post>>
    {
        private readonly DataContext _context;

        public CreatePostCommandHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<Post>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<Post>();

            try
            {
                var post = Post.CreatePost(request.UserProfileId, request.TextContent);

                _context.Posts.Add(post);
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
