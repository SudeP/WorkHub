using Api.CQRS.Users.Command;
using AutoMapper;

namespace Api.Entities.Users
{
    public class User : BaseEntity
    {
        public string GivenName { get; set; }
        public string SurName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public override void CreateMappings(Profile configuration)
        {
            base.CreateMappings(configuration);

            configuration.CreateMap<CreateUserCommand, User>();
        }
    }
}
