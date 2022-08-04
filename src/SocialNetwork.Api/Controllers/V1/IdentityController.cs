using SocialNetwork.Api.Contracts.Identity;
using SocialNetwork.Application.Identity.Commands;
using SocialNetwork.Application.Identity.Queries;

namespace SocialNetwork.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route(ApiRoutes.BaseRoute)]
    [ApiController]
    public class IdentityController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public IdentityController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        [Route(ApiRoutes.Identity.Resgistration)]
        [ValidateModel]
        public async Task<ActionResult> Register([FromBody] UserRegistration registration, CancellationToken cancellationToken)
        {
            var command = _mapper.Map<RegisterIdentityCommand>(registration);
            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsError) return HandleErrorResponse(result.Errors);

            return Ok(_mapper.Map<IdentityUserProfile>(result.Payload));
        }

        [HttpPost]
        [Route(ApiRoutes.Identity.Login)]
        [ValidateModel]
        public async Task<ActionResult> Login([FromBody] Login login, CancellationToken cancellationToken)
        {
            var command = _mapper.Map<LoginCommand>(login);
            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsError) return HandleErrorResponse(result.Errors);

            return Ok(_mapper.Map<IdentityUserProfile>(result.Payload));
        }

        [HttpDelete]
        [Route(ApiRoutes.Identity.IdentityById)]
        [ValidateGuid("identityUserId")]
        [Authorize]
        public async Task<ActionResult> DeleteAccount(string identityUserId, CancellationToken token)
        {
            var identityUserGuiId = Guid.Parse(identityUserId);
            var requestorId = HttpContext.GetIdentityIdClaimValue();

            var command = new RemoveAccountCommand 
            { 
                IdentityUserId = identityUserGuiId, 
                RequestorId = requestorId 
            };

            var result = await _mediator.Send(command, token);

            if (result.IsError) return HandleErrorResponse(result.Errors);

            return NoContent();
        }

        [HttpGet]
        [Route(ApiRoutes.Identity.CurrentUser)]
        [Authorize]
        public async Task<ActionResult> CurrentUser(CancellationToken token)
        {
            var identityId = HttpContext.GetIdentityIdClaimValue();

            var query = new GetCurrentUserQuery { IdentityId = identityId };

            var result = await _mediator.Send(query, token);
            if (result.IsError) return HandleErrorResponse(result.Errors);

            return Ok(_mapper.Map<IdentityUserProfile>(result.Payload));
        }
    }
}
