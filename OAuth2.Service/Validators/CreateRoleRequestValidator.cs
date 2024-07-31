using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OAuth2.Domain.Request;
using OAuth2.Infrastructure.Transforms;
using OAuth2.Persistence;

namespace OAuth2.Service.Validators
{
    public class CreateRoleRequestValidator : AbstractValidator<CreateRoleRequest>
    {
        public CreateRoleRequestValidator(IOAuth2DbContext pContext)
        {
            RuleFor(x => x.Name)
               .NotEmpty()
               .WithMessage(ValidatorTransform.Required(Modules.Role.Name))
               .MinimumLength(Modules.UserNameMin)
               .MustAsync(async (name, token) =>
               {
                   var exists = await pContext.Roles.FirstOrDefaultAsync(x => x.Name == name);
                   return exists == null;
               })
               .WithMessage(name => ValidatorTransform.Exists(Modules.Role.Name));

            RuleFor(x => x.Description)
                .MaximumLength(Modules.DescriptionLength)
                .WithMessage(ValidatorTransform.MaximumLength(Modules.Role.Description, Modules.DescriptionLength));
        }
    }
}
