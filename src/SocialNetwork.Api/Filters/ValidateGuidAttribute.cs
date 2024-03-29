﻿using Microsoft.AspNetCore.Mvc.Filters;
using SocialNetwork.Api.Contracts.Common;

namespace SocialNetwork.Api.Filters
{
    public class ValidateGuidAttribute : ActionFilterAttribute
    {
        private readonly List<string> _keys;

        public ValidateGuidAttribute(params string[] keys)
        {
            _keys = keys.ToList();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            bool hasError = false;
            var apiError = new ErrorResponse();
            _keys.ForEach(key =>
            {
                //if we don´t find the key with the specified value
                if (!context.ActionArguments.TryGetValue(key, out var value)) return;
                //if we are not able to parse the value to Guid
                if (!Guid.TryParse(value?.ToString(), out var guid))
                {
                    hasError = true;
                    apiError.Errors.Add($"The identifier for {key} is not correct GUID format");
                } 
            });
            
            if(hasError)
            {
                apiError.StatusCode = 400;
                apiError.StatusPhrase = "Bad Request";
                apiError.Timestamp = DateTime.Now;

                context.Result = new ObjectResult(apiError);
            }
        }
    }
}
