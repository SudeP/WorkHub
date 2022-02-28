using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.ResponseModel
{
    [Serializable]
    public enum TaskCode
    {
        Continue = 100,
        Processing = 102,
        OK = 200,
        Created = 201,
        Accepted = 202,
        NoContent = 204,
        ResetContent = 205,
        PartialContent = 206,
        Ambiguous = 300,
        Moved = 301,
        Found = 302,
        SeeOther = 303,
        NotModified = 304,
        UseProxy = 305,
        Unused = 306,
        PermanentRedirect = 308,
        BadRequest = 400,
        Unauthorized = 401,
        PaymentRequired = 402,
        Forbidden = 403,
        NotFound = 404,
        MethodNotAllowed = 405,
        NotAcceptable = 406,
        ProxyAuthenticationRequired = 407,
        RequestTimeout = 408,
        Conflict = 409,
        Gone = 410,
        LengthRequired = 411,
        PreconditionFailed = 412,
        RequestEntityTooLarge = 413,
        RequestUriTooLong = 414,
        UnsupportedMediaType = 415,
        RequestedRangeNotSatisfiable = 416,
        ExpectationFailed = 417,
        UnprocessableEntity = 422,
        Locked = 423,
        FailedDependency = 424,
        UpgradeRequired = 426,
        TooManyRequests = 429,
        InternalServerError = 500,
        NotImplemented = 501,
        BadGateway = 502,
        ServiceUnavailable = 503,
        GatewayTimeout = 504,
        LoopDetected = 508,
        NotExtended = 510,
    }

    public interface IResponseFactory
    {
        public Response<T> OK<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public Response<T> Accepted<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public Response<T> BadRequest<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public Response<T> Conflict<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public Response<T> Continue<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public Response<T> Created<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public Response<T> UnprocessableEntity<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public Response<T> ExpectationFailed<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public Response<T> Forbidden<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public Response<T> Found<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public Response<T> Gone<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public Response<T> InternalServerError<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public Response<T> MethodNotAllowed<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public Response<T> Moved<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public Response<T> NoContent<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public Response<T> NotFound<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public Response<T> Processing<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public Response<T> Unauthorized<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public Response<T> Custom<T>(TaskCode TaskCode, T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
    }
    public class ResponseFactory : IResponseFactory
    {
        public ResponseFactory(IHttpContextAccessor httpContextAccessor)
        {
            context = httpContextAccessor.HttpContext;
        }

        private readonly HttpContext context;
        public Response<T> OK<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => Custom(TaskCode.OK, resultObject, responseMessages);
        public Response<T> Accepted<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => Custom(TaskCode.Accepted, resultObject, responseMessages);
        public Response<T> BadRequest<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => Custom(TaskCode.BadRequest, resultObject, responseMessages);
        public Response<T> Conflict<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => Custom(TaskCode.Conflict, resultObject, responseMessages);
        public Response<T> Continue<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => Custom(TaskCode.Continue, resultObject, responseMessages);
        public Response<T> Created<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => Custom(TaskCode.Created, resultObject, responseMessages);
        public Response<T> UnprocessableEntity<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => Custom(TaskCode.UnprocessableEntity, resultObject, responseMessages);
        public Response<T> ExpectationFailed<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => Custom(TaskCode.ExpectationFailed, resultObject, responseMessages);
        public Response<T> Forbidden<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => Custom(TaskCode.Forbidden, resultObject, responseMessages);
        public Response<T> Found<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => Custom(TaskCode.Found, resultObject, responseMessages);
        public Response<T> Gone<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => Custom(TaskCode.Gone, resultObject, responseMessages);
        public Response<T> InternalServerError<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => Custom(TaskCode.InternalServerError, resultObject, responseMessages);
        public Response<T> MethodNotAllowed<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => Custom(TaskCode.MethodNotAllowed, resultObject, responseMessages);
        public Response<T> Moved<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => Custom(TaskCode.Moved, resultObject, responseMessages);
        public Response<T> NoContent<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => Custom(TaskCode.NoContent, resultObject, responseMessages);
        public Response<T> NotFound<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => Custom(TaskCode.NotFound, resultObject, responseMessages);
        public Response<T> Processing<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => Custom(TaskCode.Processing, resultObject, responseMessages);
        public Response<T> Unauthorized<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => Custom(TaskCode.Unauthorized, resultObject, responseMessages);
        public Response<T> Custom<T>(TaskCode taskCode, T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200;//fix

            return new Response<T>
            {
                Entity = resultObject,
                TaskCode = taskCode,
                Messages = responseMessages
            };
        }
    }
    public interface IJsonResultFactory
    {
        public JsonResult OK<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public JsonResult Accepted<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public JsonResult BadRequest<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public JsonResult Conflict<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public JsonResult Continue<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public JsonResult Created<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public JsonResult UnprocessableEntity<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public JsonResult ExpectationFailed<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public JsonResult Forbidden<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public JsonResult Found<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public JsonResult Gone<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public JsonResult InternalServerError<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public JsonResult MethodNotAllowed<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public JsonResult Moved<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public JsonResult NoContent<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public JsonResult NotFound<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public JsonResult Processing<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public JsonResult Unauthorized<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public JsonResult Custom<T>(TaskCode TaskCode, T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
    }
    public class JsonResultFactory : IJsonResultFactory
    {
        public JsonResultFactory(IHttpContextAccessor httpContextAccessor)
        {
            context = httpContextAccessor.HttpContext;
        }

        private readonly HttpContext context;

        public JsonResult OK<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => Custom(TaskCode.OK, resultObject, responseMessages);
        public JsonResult Accepted<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => Custom(TaskCode.Accepted, resultObject, responseMessages);
        public JsonResult BadRequest<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => Custom(TaskCode.BadRequest, resultObject, responseMessages);
        public JsonResult Conflict<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => Custom(TaskCode.Conflict, resultObject, responseMessages);
        public JsonResult Continue<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => Custom(TaskCode.Continue, resultObject, responseMessages);
        public JsonResult Created<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => Custom(TaskCode.Created, resultObject, responseMessages);
        public JsonResult UnprocessableEntity<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => Custom(TaskCode.UnprocessableEntity, resultObject, responseMessages);
        public JsonResult ExpectationFailed<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => Custom(TaskCode.ExpectationFailed, resultObject, responseMessages);
        public JsonResult Forbidden<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => Custom(TaskCode.Forbidden, resultObject, responseMessages);
        public JsonResult Found<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => Custom(TaskCode.Found, resultObject, responseMessages);
        public JsonResult Gone<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => Custom(TaskCode.Gone, resultObject, responseMessages);
        public JsonResult InternalServerError<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => Custom(TaskCode.InternalServerError, resultObject, responseMessages);
        public JsonResult MethodNotAllowed<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => Custom(TaskCode.MethodNotAllowed, resultObject, responseMessages);
        public JsonResult Moved<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => Custom(TaskCode.Moved, resultObject, responseMessages);
        public JsonResult NoContent<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => Custom(TaskCode.NoContent, resultObject, responseMessages);
        public JsonResult NotFound<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => Custom(TaskCode.NotFound, resultObject, responseMessages);
        public JsonResult Processing<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => Custom(TaskCode.Processing, resultObject, responseMessages);
        public JsonResult Unauthorized<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => Custom(TaskCode.Unauthorized, resultObject, responseMessages);
        public JsonResult Custom<T>(TaskCode TaskCode, T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
        {
            JsonResult result = new(new Response<T>
            {
                Entity = resultObject,
                TaskCode = TaskCode,
                Messages = responseMessages
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)TaskCode;
            result.ContentType = "application/json";
            result.StatusCode = (int)TaskCode;

            return result;
        }
    }

    public class Response<T>
    {
        [EnumDataType(typeof(TaskCode))]
        [JsonConverter(typeof(StringEnumConverter))]
        public TaskCode TaskCode { get; set; }
        public T Entity { get; set; }
        public IEnumerable<ResponseMessage> Messages { get; set; }
    }

    public struct ResponseMessage
    {
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }
}