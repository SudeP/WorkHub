using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
#pragma warning disable
    public class CoreController : Controller
    {
        private IMediator _mediator;
        protected IMediator mediator => _mediator ?? (_mediator = HttpContext.RequestServices.GetService<IMediator>());

        protected IRequest<T> FillServices<T>(IRequest<T> obj)
        {
            obj
                .GetType()
                .GetProperties()
                .Where(x => x.PropertyType.IsInterface)
                .ToList()
                .ForEach(x => x.SetValue(obj, HttpContext.RequestServices.GetService(x.PropertyType)));

            return obj;
        }
    }
#pragma warning restore
}