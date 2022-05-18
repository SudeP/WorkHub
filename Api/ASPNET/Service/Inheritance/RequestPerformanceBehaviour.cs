using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Api.ASPNET.Service.Inheritance
{
    public class RequestPerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;

        public RequestPerformanceBehaviour(ILogger<TRequest> logger)
        {
            _timer = new Stopwatch();
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            //_timer.Start();
            //var name = typeof(TRequest).Name;

            var response = await next();
            ////_unitOfWork.GetWriteRepository<RequestLogInfo>().Insert(new RequestLogInfo()
            ////{
            ////    Url = name,
            ////    RequestJson = JsonConvert.SerializeObject(request),
            ////    ResponseJson = JsonConvert.SerializeObject(response)
            ////    //CreateBy = !String.IsNullOrEmpty(customerId) ? Convert.ToInt32(customerId) : 0
            ////});

            //_timer.Stop();

            //if (_timer.ElapsedMilliseconds > 60000)
            //{
            //    //var name = typeof(TRequest).Name;

            //    // TODO: Add User Details 
            //    _logger.LogWarning("Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@Request}", name, _timer.ElapsedMilliseconds, request);
            //}

            return response;
        }
    }
}
