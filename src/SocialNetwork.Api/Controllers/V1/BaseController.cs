﻿using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Api.Contracts.Common;
using SocialNetwork.Application.Enums;
using SocialNetwork.Application.Models;

namespace SocialNetwork.Api.Controllers.V1
{
    public class BaseController : ControllerBase
    {
        protected ActionResult HandleErrorResponse(IEnumerable<Error> errors)
        {
            var apiError = new ErrorResponse();

            if (errors.Any(e => e.Code == ErrorCode.NotFound))
            {
                var error = errors.FirstOrDefault(e => e.Code == ErrorCode.NotFound);

                apiError.StatusCode = 404;
                apiError.StatusPhrase = "Not Found";
                apiError.Timestamp = DateTime.Now;
                apiError.Errors.Add(error.Message);

                return NotFound(apiError);
            }

            apiError.StatusCode = 500;
            apiError.StatusPhrase = "Internal server Error";
            apiError.Timestamp = DateTime.Now;
            apiError.Errors.Add("Unknown error");

            return StatusCode(500, apiError);
        }
    }
}
