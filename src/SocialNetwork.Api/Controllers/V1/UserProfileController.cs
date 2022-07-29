using SocialNetwork.Api.Contracts.UserProfiles.Requests;
using SocialNetwork.Api.Contracts.UserProfiles.Responses;
using SocialNetwork.Application.UserProfiles.Commands;
using SocialNetwork.Application.UserProfiles.Queries;

namespace SocialNetwork.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route(ApiRoutes.BaseRoute)]
    [ApiController]
    [Authorize]
    public class UserProfileController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public UserProfileController(IMediator mediator, 
                                     IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllProfiles(CancellationToken cancellationToken)
        {
            var query = new GetAllUserProfileQuery();
            var response = await _mediator.Send(query, cancellationToken);
            var profiles = _mapper.Map<List<UserProfileResponse>>(response.Payload);

            return Ok(profiles);
        }
         
        [HttpGet]
        [Route(ApiRoutes.UserProfile.IdRoute)]
        [ValidateGuid("{id}")]
        public async Task<ActionResult> GetUserProfileById(string id, CancellationToken cancellationToken)
        {
            var query = new GetUserProfileByIdQuery { UserProfileId = Guid.Parse(id) };
            var response = await _mediator.Send(query, cancellationToken);

            if (response.IsError) return HandleErrorResponse(response.Errors);

            var userProfile = _mapper.Map<UserProfileResponse>(response.Payload);

            return Ok(userProfile);
        }

        [HttpPatch]
        [Route(ApiRoutes.UserProfile.IdRoute)]
        [ValidateGuid("id")]
        [ValidateModel]
        public async Task<ActionResult> UpdateUserProfile(string id, UserProfileCreate updateUserProfile, 
            CancellationToken cancellationToken)
        {
            var command = _mapper.Map<UpdateUserProfileBasicInfoCommand>(updateUserProfile);
            command.UserProfileId = Guid.Parse(id);
            var response = await _mediator.Send(command, cancellationToken);

            if (response.IsError) return HandleErrorResponse(response.Errors);

            return NoContent();
        }
    }
}
