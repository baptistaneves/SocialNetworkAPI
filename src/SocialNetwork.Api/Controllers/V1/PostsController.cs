using SocialNetwork.Api.Contracts.Common;
using SocialNetwork.Api.Contracts.Posts.Requests;
using SocialNetwork.Api.Contracts.Posts.Responses;
using SocialNetwork.Application.Posts.Commands;
using SocialNetwork.Application.Posts.Queries;

namespace SocialNetwork.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route(ApiRoutes.BaseRoute)]
    [ApiController]
    [Authorize]
    public class PostsController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public PostsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllPosts(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllPostsQuery(), cancellationToken);

            return result.IsError ? HandleErrorResponse(result.Errors)
                : Ok(_mapper.Map<List<PostResponse>>(result.Payload));
        }

        [HttpGet]
        [Route(ApiRoutes.Post.IdRoute)]
        [ValidateGuid("{id}")]
        public async Task<ActionResult> GetPostById(string id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetPostByIdQuery { PostId = Guid.Parse(id)}, cancellationToken);

            return result.IsError ? HandleErrorResponse(result.Errors)
                : Ok(_mapper.Map<PostResponse>(result.Payload));
        }

        [HttpDelete]
        [Route(ApiRoutes.Post.IdRoute)]
        [ValidateGuid("{id}")]
        public async Task<ActionResult> DeletePost(string id, CancellationToken cancellationToken)
        {
            var userProfileId = HttpContext.GetUserProfileIdClaimValue();

            var command = new DeletePostCommand { PostId = Guid.Parse(id), UserProfileId = userProfileId };
            var result = await _mediator.Send(command, cancellationToken);

            return result.IsError ? HandleErrorResponse(result.Errors) : NoContent();
        }

        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult> CreatePost([FromBody] PostCreate newPost, CancellationToken cancellationToken)
        {
            var userProfileId = HttpContext.GetUserProfileIdClaimValue();

            var command = new CreatePostCommand()
            {
                UserProfileId = userProfileId,
                TextContent = newPost.TextContent
            };

            var result = await _mediator.Send(command, cancellationToken);
            var response = _mapper.Map<PostResponse>(result.Payload);

            return result.IsError ? HandleErrorResponse(result.Errors) :
                CreatedAtAction(nameof(GetPostById), new { id = response.PostId }, response);
        }

        [HttpPatch]
        [Route(ApiRoutes.Post.IdRoute)]
        [ValidateGuid("{id}")]
        [ValidateModel]
        public async Task<ActionResult> UpdatePostContent(string id, [FromBody] PostUpdate updatedPost, CancellationToken cancellationToken)
        {
            var userProfileId = HttpContext.GetUserProfileIdClaimValue();

            var command = new UpdatePostContentCommand()
            {
                NewText = updatedPost.TextContent,
                PostId = Guid.Parse(id),
                UserProfileId = userProfileId
            };

            var response = await _mediator.Send(command, cancellationToken);

            if (response.IsError) return HandleErrorResponse(response.Errors);

            return NoContent();
        }

        [HttpGet]
        [Route(ApiRoutes.Post.PostComments)]
        [ValidateGuid("{postId}")]
        public async Task<ActionResult> GetCommentsByPostId(string postId, CancellationToken cancellationToken)
        {
            var query = new GetPostCommentsQuery() { PostId = Guid.Parse(postId) };
            var result = await _mediator.Send(query, cancellationToken);

            if (result.IsError) HandleErrorResponse(result.Errors);

            var comments =_mapper.Map<List<PostCommentResponse>>(result.Payload);
            return Ok(comments);
        }

        [HttpPost]
        [Route(ApiRoutes.Post.PostComments)]
        [ValidateGuid("{postId}")]
        [ValidateModel]
        public async Task<ActionResult> AddCommentToPost(string postId, [FromBody] PostCommentCreate comment, 
            CancellationToken cancellationToken)
        {
            var isValidGuid = Guid.TryParse(comment.UserProfileId , out var userProfileId);

            if(!isValidGuid)
            {
                var apiError = new ErrorResponse();

                apiError.StatusCode = 400;
                apiError.StatusPhrase = "Bad Request";
                apiError.Timestamp = DateTime.Now;
                apiError.Errors.Add("The provided User profile ID is not a valid Guid format");

                return BadRequest(apiError);
            }

            var query = new AddPostCommentCommand()
            {
                PostId = Guid.Parse(postId),
                UserProfileId = userProfileId,
                CommentText = comment.Text
            };

            var result = await _mediator.Send(query, cancellationToken);
            if(result.IsError) return HandleErrorResponse(result.Errors);

            var newComment = _mapper.Map<PostCommentResponse>(result.Payload);

            return Ok(newComment);
        }
    }
}
