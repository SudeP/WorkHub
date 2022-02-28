using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
#pragma warning disable
    public class BaseController : Controller
    {
        //protected readonly HttpContext context;
        //protected readonly IConfiguration configuration;
        private IMediator _mediator;
        protected IMediator mediator => _mediator ?? (_mediator = HttpContext.RequestServices.GetService<IMediator>());
    }
#pragma warning restore
}