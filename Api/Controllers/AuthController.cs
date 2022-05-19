using Api.Controllers.CQRS.Users.Command;
using Api.Models.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers
{
    /// <summary>
    /// Token işlemlerini yönetir.
    /// </summary>
    public class AuthController : CoreController
    {
        /// <summary>
        /// Token üret
        /// </summary>
        /// <remarks>
        /// Api'yi kullanabilmek için token üretir
        /// </remarks>
        /// <param name="body">Giriş yapma bilgileri.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result<string>> Post([FromBody] SignInUserQuery body)
        {
            return await mediator.Send(body);
        }
    }
}
