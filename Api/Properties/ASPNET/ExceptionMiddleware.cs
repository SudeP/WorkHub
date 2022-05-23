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
        public override void OnException(ExceptionContext context)
        {
            context.ExceptionHandled = true;

            var ex = context.Exception;

            if (ex is ValidationException vex)
            {
                List<Information> messages = new();

                foreach (ValidationFailure error in vex.Errors)
                {
                    messages.Add(new Information
                    {
                        Message = error.ErrorMessage,
                        Detail = error.Severity.ToString()
                    });
                }

                context.Result = new JsonResult(new Result<object> { Status = System.Net.HttpStatusCode.InternalServerError, Infos = messages });
            }
            else
            {
                context.Result = new JsonResult(new Result<object>
                {
                    Status = System.Net.HttpStatusCode.InternalServerError,
                    Infos = new List<Information>
                    {
                        new Information
                        {
                            Message = context.Exception.Message,
                            Detail = context.Exception.StackTrace
                        }
                    }
                });
            }
        }
    }
}
