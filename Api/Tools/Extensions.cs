using Api.Models.ResponseModel;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

#pragma warning disable
public static class Extensions
{
    public static Session GetSession(this IDictionary<object, object?> items)
    {
        items.TryGetValue("currentSession", out object value);

        return (Session)value;
    }

    public static ISession GetSession(this HttpContext httpContext)
    {
        return httpContext.RequestServices.GetService<ISession>();
    }

    public static T GetService<T>(this HttpContext httpContext)
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