using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Api.Models.ResponseModel
{
    public class Result<T>
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
        public Task<Result<T>> OK<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public Task<Result<T>> Accepted<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public Task<Result<T>> BadRequest<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public Task<Result<T>> Conflict<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public Task<Result<T>> Continue<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public Task<Result<T>> Created<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public Task<Result<T>> UnprocessableEntity<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public Task<Result<T>> ExpectationFailed<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public Task<Result<T>> Forbidden<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public Task<Result<T>> Found<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public Task<Result<T>> Gone<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public Task<Result<T>> InternalServerError<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public Task<Result<T>> MethodNotAllowed<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public Task<Result<T>> Moved<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public Task<Result<T>> NoContent<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public Task<Result<T>> NotFound<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public Task<Result<T>> Processing<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public Task<Result<T>> Unauthorized<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
        public Task<Result<T>> Custom<T>(TaskCode TaskCode, T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null);
    }

    public class ResponseFactory : IResponseFactory
    {
        public ResponseFactory(IHttpContextAccessor httpContextAccessor)
        {
            context = httpContextAccessor.HttpContext;
        }
        private readonly HttpContext context;
        public async Task<Result<object>> OK(string message)
            => await Custom<object>(TaskCode.OK, null, new List<ResponseMessage> { new ResponseMessage { Message = message } });
        public async Task<Result<object>> Accepted(ResponseMessage responseMessage)
            => await Custom<object>(TaskCode.OK, null, new List<ResponseMessage> { responseMessage });
        public async Task<Result<object>> BadRequest(ResponseMessage responseMessage)
            => await Custom<object>(TaskCode.OK, null, new List<ResponseMessage> { responseMessage });
        public async Task<Result<object>> Conflict(ResponseMessage responseMessage)
            => await Custom<object>(TaskCode.OK, null, new List<ResponseMessage> { responseMessage });
        public async Task<Result<object>> Continue(ResponseMessage responseMessage)
            => await Custom<object>(TaskCode.OK, null, new List<ResponseMessage> { responseMessage });
        public async Task<Result<object>> Created(ResponseMessage responseMessage)
            => await Custom<object>(TaskCode.OK, null, new List<ResponseMessage> { responseMessage });
        public async Task<Result<object>> UnprocessableEntity(ResponseMessage responseMessage)
            => await Custom<object>(TaskCode.OK, null, new List<ResponseMessage> { responseMessage });
        public async Task<Result<object>> ExpectationFailed(ResponseMessage responseMessage)
            => await Custom<object>(TaskCode.OK, null, new List<ResponseMessage> { responseMessage });
        public async Task<Result<object>> Forbidden(ResponseMessage responseMessage)
            => await Custom<object>(TaskCode.OK, null, new List<ResponseMessage> { responseMessage });
        public async Task<Result<object>> Found(ResponseMessage responseMessage)
            => await Custom<object>(TaskCode.OK, null, new List<ResponseMessage> { responseMessage });
        public async Task<Result<object>> Gone(ResponseMessage responseMessage)
            => await Custom<object>(TaskCode.OK, null, new List<ResponseMessage> { responseMessage });
        public async Task<Result<object>> InternalServerError(ResponseMessage responseMessage)
            => await Custom<object>(TaskCode.OK, null, new List<ResponseMessage> { responseMessage });
        public async Task<Result<object>> MethodNotAllowed(ResponseMessage responseMessage)
            => await Custom<object>(TaskCode.OK, null, new List<ResponseMessage> { responseMessage });
        public async Task<Result<object>> Moved(ResponseMessage responseMessage)
            => await Custom<object>(TaskCode.OK, null, new List<ResponseMessage> { responseMessage });
        public async Task<Result<object>> NoContent(ResponseMessage responseMessage)
            => await Custom<object>(TaskCode.OK, null, new List<ResponseMessage> { responseMessage });
        public async Task<Result<object>> NotFound(ResponseMessage responseMessage)
            => await Custom<object>(TaskCode.OK, null, new List<ResponseMessage> { responseMessage });
        public async Task<Result<object>> Processing(ResponseMessage responseMessage)
            => await Custom<object>(TaskCode.OK, null, new List<ResponseMessage> { responseMessage });
        public async Task<Result<object>> Unauthorized(ResponseMessage responseMessage)
            => await Custom<object>(TaskCode.OK, null, new List<ResponseMessage> { responseMessage });
        public async Task<Result<object>> Custom(ResponseMessage responseMessage)
            => await Custom<object>(TaskCode.OK, null, new List<ResponseMessage> { responseMessage });
        public async Task<Result<T>> OK<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => await Custom(TaskCode.OK, resultObject, responseMessages);
        public async Task<Result<T>> Accepted<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => await Custom(TaskCode.Accepted, resultObject, responseMessages);
        public async Task<Result<T>> BadRequest<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => await Custom(TaskCode.BadRequest, resultObject, responseMessages);
        public async Task<Result<T>> Conflict<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => await Custom(TaskCode.Conflict, resultObject, responseMessages);
        public async Task<Result<T>> Continue<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => await Custom(TaskCode.Continue, resultObject, responseMessages);
        public async Task<Result<T>> Created<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => await Custom(TaskCode.Created, resultObject, responseMessages);
        public async Task<Result<T>> UnprocessableEntity<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => await Custom(TaskCode.UnprocessableEntity, resultObject, responseMessages);
        public async Task<Result<T>> ExpectationFailed<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => await Custom(TaskCode.ExpectationFailed, resultObject, responseMessages);
        public async Task<Result<T>> Forbidden<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => await Custom(TaskCode.Forbidden, resultObject, responseMessages);
        public async Task<Result<T>> Found<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => await Custom(TaskCode.Found, resultObject, responseMessages);
        public async Task<Result<T>> Gone<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => await Custom(TaskCode.Gone, resultObject, responseMessages);
        public async Task<Result<T>> InternalServerError<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => await Custom(TaskCode.InternalServerError, resultObject, responseMessages);
        public async Task<Result<T>> MethodNotAllowed<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => await Custom(TaskCode.MethodNotAllowed, resultObject, responseMessages);
        public async Task<Result<T>> Moved<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => await Custom(TaskCode.Moved, resultObject, responseMessages);
        public async Task<Result<T>> NoContent<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => await Custom(TaskCode.NoContent, resultObject, responseMessages);
        public async Task<Result<T>> NotFound<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => await Custom(TaskCode.NotFound, resultObject, responseMessages);
        public async Task<Result<T>> Processing<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => await Custom(TaskCode.Processing, resultObject, responseMessages);
        public async Task<Result<T>> Unauthorized<T>(T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
            => await Custom(TaskCode.Unauthorized, resultObject, responseMessages);
        public async Task<Result<T>> Custom<T>(TaskCode taskCode, T resultObject = default, IEnumerable<ResponseMessage> responseMessages = null)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200;//fix

            return await Task.FromResult(new Result<T>
            {
                Entity = resultObject,
                TaskCode = taskCode,
                Messages = responseMessages
            });
        }
    }
}