using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Domain.Models;

namespace SocialNetwork.Api.Controllers.V2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        [HttpGet]
        [Route("{id}")]
        public ActionResult<Post> GetById(int id)
        {
            return Ok(new Post{ Id = id, Text = "Hello, Universe!" });
        }
    }
}
