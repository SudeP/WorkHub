using Api.Models.ResponseModel;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;

namespace Api.Properties.ASPNET
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class ExceptionMiddleware : ExceptionFilterAttribute
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
                List<ResponseMessage> messages = new();

                foreach (ValidationFailure error in vex.Errors)
                {
                    messages.Add(new ResponseMessage
                    {
                        Message = error.ErrorMessage,
                        StackTrace = error.Severity.ToString()
                    });
                }

                context.Result = new JsonResult(response.InternalServerError<object>(responseMessages: messages));
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
