using Api;
using Api.ASPNET.Middleware.Exception;
using Api.Models.ResponseModel;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;

namespace Api.ASPNET.Service.Middleware
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ExceptionMiddleware : ExceptionFilterAttribute
    {
        private readonly IResponseFactory response;

        public ExceptionMiddleware(IResponseFactory responseFactory)
        {
            response = responseFactory;
        }

        public override void OnException(ExceptionContext context)
        {
            context.ExceptionHandled = true;

            var ex = context.Exception;

            if (ex is ValidationException vex)
            {
                context.Result = new JsonResult(response.InternalServerError<object>(responseMessages: vex.Errors.Serialize()));
            }
            else
            {
                context.Result = new JsonResult(response.InternalServerError<object>(responseMessages: new List<ResponseMessage>
                {
                    new ResponseMessage
                    {
                        Message = context.Exception.Message,
                        StackTrace = context.Exception.StackTrace
                    }
                }));
            }
        }
    }
}
