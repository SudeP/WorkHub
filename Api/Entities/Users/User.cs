using Api.ASPNET.Service.Inheritance;
using Api.CQRS.Users.Command;
using AutoMapper;

namespace Api.Entities.Users
{
    public class User : BaseEntity, IHaveCustomMapping
    {
        public string GivenName { get; set; }
        public string SurName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<CreateUserCommand, User>();
        }
    }
}
