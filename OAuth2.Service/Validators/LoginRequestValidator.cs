using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OAuth2.Domain.Request;
using OAuth2.Infrastructure.Custom;
using OAuth2.Infrastructure.Transforms;
using OAuth2.Persistence;

namespace OAuth2.Service.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator(IOAuth2DbContext pContext)
        {
            RuleFor(x => x.Email)
                .Must((model, email) => model.IsEmail == false || (string.IsNullOrEmpty(email) == false
                                && ValidatorCustom.BeValidEmail(email)))
                .WithMessage(ValidatorTransform.ValidValue(Modules.User.Email))
                .MustAsync(async (model, email, token) =>
                {
                    if (model.IsEmail == false)
                        return true;

                    var exists = await pContext.Users.FirstOrDefaultAsync(x => x.Email == email);
                    return exists == null;
                })
                .WithMessage(email => ValidatorTransform.Exists(Modules.User.Email));

            RuleFor(x => x.PhoneNumber)
                .Must((model, phoneNumber) => model.IsEmail || (string.IsNullOrEmpty(phoneNumber) == false
                                && phoneNumber.Length == Modules.PhoneNumberLength))
                .WithMessage(ValidatorTransform.Length(Modules.User.PhoneNumber, Modules.PhoneNumberLength))
                .MustAsync(async (model, phoneNumber, token) =>
                {
                    if (model.IsEmail)
                        return true;

                    var exists = await pContext.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
                    return exists == null;
                })
                .WithMessage(phoneNumber => ValidatorTransform.Exists(Modules.User.PhoneNumber));

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(ValidatorTransform.Required(Modules.User.Password));
        }
    }
}
