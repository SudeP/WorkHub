using Api.Controllers.CQRS.Users.Command;
using Api.Models.Structs;
using AutoMapper;

namespace Api.Models.Entities.Users
{
    public class User : Entity, IEntity
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsApproved { get; set; }

        public override void CreateMappings(Profile configuration)
        {
            base.CreateMappings(configuration);

            configuration.CreateMap<CreateUserCommand, User>();
        }
    }
}