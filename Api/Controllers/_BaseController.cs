using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [Authorize]
#pragma warning disable
    public class BaseController : CoreController
    {
    }
#pragma warning restore
}