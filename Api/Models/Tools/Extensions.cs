using Api.Models.ResponseModel;
using Api.Models.Tools;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

#pragma warning disable
public static class Extensions
{
    public static RequestSession GetSession(this IDictionary<object, object?> items)
    {
        items.TryGetValue("currentSession", out object value);

        return (RequestSession)value;
    }

    public static IRequestSession GetSession(this Microsoft.AspNetCore.Http.HttpContext httpContext)
    {
        return httpContext.RequestServices.GetService<IRequestSession>();
    }

    public static T GetService<T>(this Microsoft.AspNetCore.Http.HttpContext httpContext)
    {
        return httpContext.RequestServices.GetService<T>();
    }

    public static IEnumerable<ResponseMessage> Serialize(this IEnumerable<ValidationFailure> errors)
    {
        List<ResponseMessage> messages = new();

        foreach (ValidationFailure error in errors)
        {
            messages.Add(new ResponseMessage
            {
                Message = error.ErrorMessage,
                StackTrace = error.Severity.ToString()
            });
        }

        return messages;
    }
}
#pragma warning restore