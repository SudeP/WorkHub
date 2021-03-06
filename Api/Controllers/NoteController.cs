using Api.Controllers.CQRS.Users.Command;
using Api.Models.ResponseModel;
using Api.Models.ResponseModel.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers
{
    /// <summary>
    /// Kullanıcı işlemlerini yönetir.
    /// </summary>
    public class NoteController : BaseController
    {
        /// <summary>
        /// Yeni Kullanıcı oluştur
        /// </summary>
        /// <remarks>
        /// Belirtilen bilgiler ile yeni bir kullanıcı oluşturulur.
        /// </remarks>
        /// <param name="body">Kullanıcının bilgileri.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result<ResultCreate>> Post([FromBody] CreateUserCommand body)
        {
            return await mediator.Send(body);
        }
    }
}