using Api.Models.ResponseModel;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Models.Sturcts.Inheritances
{
    public abstract class RequestMiddlewareHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);

        public FileContentResult File(byte[] fileContents, string contentType, string downloadName)
        {
            return new FileContentResult(fileContents, contentType)
            {
                FileDownloadName = downloadName
            };
        }

        public async Task<Result<T>> Custom<T>(HttpStatusCode httpStatusCode, T resultObject = default, params Information[] responseMessages)
        {
            return await Task.FromResult(new Result<T>
            {
                Entity = resultObject,
                Status = httpStatusCode,
                Infos = responseMessages
            });
        }
    }
}