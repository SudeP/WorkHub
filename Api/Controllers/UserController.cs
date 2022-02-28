using Api.CQRS.Users.Command;
using Api.Models.ResponseModel;
using Api.Models.ResponseModel.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers
{
    /// <summary>
    /// Kullanıcı işlemlerini yönetir.
    /// </summary>
    public class UserController : BaseController
    {
        /// <summary>
        /// Token üret
        /// </summary>
        /// <remarks>
        /// Api'yi kullanabilmek için token üretir
        /// </remarks>
        /// <param name="command">Giriş yapma bilgileri.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result<string>> SignIn([FromBody] SignInUserCommand command)
        {
            return await mediator.Send(command);
        }

        /// <summary>
        /// Yeni Kullanıcı oluştur
        /// </summary>
        /// <remarks>
        /// Belirtilen bilgiler ile yeni bir kullanıcı oluşturulur.
        /// </remarks>
        /// <param name="command">Kullanıcının bilgileri.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result<ResultCreate>> Post([FromBody] CreateUserCommand command)
        {
            return await mediator.Send(command);
        }
    }
}
