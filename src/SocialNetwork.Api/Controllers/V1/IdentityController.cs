using SocialNetwork.Api.Contracts.Identity;
using SocialNetwork.Application.Identity.Commands;

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
        public async Task<ActionResult> Register([FromBody] UserRegistration registration)
        {
            var command = _mapper.Map<RegisterIdentityCommand>(registration);
            var result = await _mediator.Send(command);

            if (result.IsError) return HandleErrorResponse(result.Errors);

            var authenticationResult = new AuthenticationResult() { Token = result.Payload };

            return Ok(authenticationResult);
        }
    }
}
