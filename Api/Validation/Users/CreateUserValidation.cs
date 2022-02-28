using Api.CQRS.Users.Command;
using FluentValidation;

namespace Api.Validation.Users
{
    public class CreateUserValidation : AbstractValidator<CreateUserCommand>
    {
        public CreateUserValidation()
        {
            RuleFor(x => x.UserName)
                .NotNull()
                .MinimumLength(5)
                .MaximumLength(20)
                .Matches("reguler expression")
                .WithMessage("Kullanıcı adı rakam ve ingiliz alfabesi harfleri içermelidir");

            //RuleFor(x => x.Email)
            //    .NotNull()
            //    .WithMessage("E-mail adı boş geçilemez !")
            //    .EmailAddress()
            //    ;
        }
    }
}
