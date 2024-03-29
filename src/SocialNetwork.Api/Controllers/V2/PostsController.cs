﻿using Microsoft.AspNetCore.Mvc;

namespace SocialNetwork.Api.Controllers.V2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        [HttpGet]
        [Route("{id}")]
        public ActionResult GetById(int id)
        {
            return Ok();
        }
    }
}
