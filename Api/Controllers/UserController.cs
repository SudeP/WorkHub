using Api.Controllers.CQRS.Users.Command;
using Api.Models.ResponseModel;
using Api.Models.ResponseModel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    /// <summary>
    /// Kullanıcı işlemlerini yönetir
    /// </summary>
    public class UserController : BaseController
    {
        /// <summary>
        /// Kullanıcı oluştur
        /// </summary>
        /// <remarks>
        /// Belirtilen bilgiler ile yeni bir kullanıcı oluşturulur
        /// </remarks>
        /// <param name="body">Kullanıcının bilgileri</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<Result<ResultCreate>> Post([FromBody] CreateUserCommand body)
        {
            return await mediator.Send(body);
        }

        /// <summary>
        /// Kullanıcı güncelle
        /// </summary>
        /// <remarks>
        /// Belirtilen tekil bilgi ile kullanıcının verilerini günceller
        /// </remarks>
        /// <param name="body">Kullanıcının bilgileri</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPut]
        public async Task<Result<ResultUpdate>> Put([FromBody] UpdateUserCommand body) => await mediator.Send(FillServices(body));
    }
}