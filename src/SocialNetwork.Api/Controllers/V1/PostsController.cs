using Microsoft.AspNetCore.Mvc;

namespace SocialNetwork.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route(ApiRoutes.BaseRoute)]
    [ApiController]
    public class PostsController : ControllerBase
    {
        [HttpGet]
        [Route(ApiRoutes.Post.GetById)]
        public ActionResult GetById(int id)
        {
            return Ok();
        }
    }
}
