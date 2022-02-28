using Api.CQRS.Users.Command;
using Api.Entities.Users;
using Api.Models.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;

namespace Api.Controllers
{
    public class UserController : BaseController
    {
        /// <summary>
        /// Create user
        /// </summary>
        [HttpPost]
        [AllowAnonymous]//bu kaldırılacak
        [SwaggerOperation(Summary = "Kullanıcı oluştur", Description = "Belirtilen bilgiler ile bir kullanıcı oluşturulur.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Kullanıcı oluşturma cevabı", typeof(Response<object>), "application/json")]
        public async Task<Response<object>> Post(
            [SwaggerRequestBody("Oluşturulacak kullanıcının gerekli olan bilgilerini içerir.", Required = true)]
            [FromBody] CreateUserCommand command)
        {
            return await mediator.Send(command);
        }

        /// <summary>
        /// User login endpoint
        /// </summary>
        [HttpGet("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AllowAnonymous]
        public IActionResult Login()
        {
            var session = HttpContext
                .GetSession();

            //Login Logic
            // ...

            var user = new User() { Id = 312 };

            session.userId = user.Id;

            string createdToken = session.CreateToken(TimeSpan.FromDays(1));

            HttpContext.Response.Headers.Remove("Authorization");
            HttpContext.Response.Headers.Add("Authorization", createdToken);
            HttpContext.Response.Headers.Remove("Access-Control-Expose-Headers");
            HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Authorization");

            return Ok();
        }
    }
}
