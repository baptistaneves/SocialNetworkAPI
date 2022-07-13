﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SocialNetwork.Api.Contracts.Common;

namespace SocialNetwork.Api.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting (ResultExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var apiError = new ErrorResponse();

                apiError.StatusCode = 400;
                apiError.StatusPhrase = "Bad Request";
                apiError.Timestamp = DateTime.Now;

                var errors = context.ModelState.AsEnumerable();

                foreach (var error in errors)
                {
                    apiError.Errors.Add(error.Value.ToString());
                }

                context.Result = new BadRequestObjectResult(apiError);
            }
        }
    }
}
