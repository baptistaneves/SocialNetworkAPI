using MediatR;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Api.Contracts.UserProfiles.Requests;

namespace SocialNetwork.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route(ApiRoutes.BaseRoute)]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllProfiles()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> CreateUserProfile([FromBody] UserProfileCreate profile)
        {
            
        }
    }
}
