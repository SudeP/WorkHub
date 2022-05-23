using Api.Controllers.CQRS._Develops.Command;
using Api.Models.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers
{
    /// <summary>
    /// Test ve bazı basit işlemler için
    /// </summary>
    /// 
    [AllowAnonymous]
#pragma warning disable
    public class DevelopController : CoreController
    {
        /// <summary>
        /// Token üret
        /// </summary>
        /// <remarks>
        /// Api'yi kullanabilmek için token üretir
        /// </remarks>
        /// <param name="command">Giriş yapma bilgileri.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result<string>> Hello()
        {
            return new Result<string> { Entity = "Hellllo siiir !" };
        }

        /// <summary>
        /// Mongo tablo migration sistemi. Özel
        /// </summary>
        /// <param name="body">Sistem kullanıcı bilgileri</param>
        /// <returns>İşlem başarılı sonucu döner...</returns>
        [HttpPost]
        public async Task<Result<string>> Post([FromBody] MongoMigrationCommand body)
        {
            return await mediator.Send(body);
        }
    }
}
