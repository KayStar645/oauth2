using FluentValidation;
using k8s.KubeConfigModels;
using Microsoft.EntityFrameworkCore;
using OAuth2.Domain.Request;
using OAuth2.Infrastructure.Custom;
using OAuth2.Infrastructure.Transforms;
using OAuth2.Persistence;

namespace OAuth2.Infrastructure.Validators
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator(IOAuth2DbContext pContext) 
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage(ValidatorTransform.Required(Modules.User.FirstName))
                .MinimumLength(Modules.NameMin)
                .WithMessage(ValidatorTransform.MinimumLength(Modules.User.FirstName, Modules.NameMin))
                .MaximumLength(Modules.NameMax)
                .WithMessage(ValidatorTransform.MaximumLength(Modules.User.FirstName, Modules.NameMax));

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage(ValidatorTransform.Required(Modules.User.LastName))
                .MinimumLength(Modules.NameMin)
                .WithMessage(ValidatorTransform.MinimumLength(Modules.User.LastName, Modules.NameMin))
                .MaximumLength(Modules.NameMax)
                .WithMessage(ValidatorTransform.MaximumLength(Modules.User.LastName, Modules.NameMax));

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
                    if (model.IsEmail == true)
                        return true;

                    var exists = await pContext.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
                    return exists == null;
                })
                .WithMessage(phoneNumber => ValidatorTransform.Exists(Modules.User.PhoneNumber));

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(ValidatorTransform.Required(Modules.User.Password))
                .MinimumLength(Modules.PasswordMin)
                .WithMessage(ValidatorTransform.MinimumLength(Modules.User.Password, Modules.PasswordMin))
                .MaximumLength(Modules.PasswordMax)
                .WithMessage(ValidatorTransform.MaximumLength(Modules.User.Password, Modules.PasswordMax));

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .WithMessage(ValidatorTransform.Required(Modules.User.ConfirmPassword))
                .Equal(x => x.Password)
                .WithMessage(ValidatorTransform.Equal(Modules.User.ConfirmPassword, Modules.User.Password));
        }
    }
}
