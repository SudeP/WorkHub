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
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(20)
                .Matches(@"[a-zA-ZşıçöüğŞİÇÖÜĞ]*")
                .Matches(@"^[a-zA-ZşıçöüğŞİÇÖÜĞ ]*$");

            RuleFor(x => x.Password)
                .NotNull()
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(16)
                .Matches(@"^\S*$");

            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                .EmailAddress();
        }
    }
}