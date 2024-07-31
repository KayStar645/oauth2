using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OAuth2.Domain.Request;
using OAuth2.Infrastructure.Transforms;
using OAuth2.Persistence;

namespace OAuth2.Service.Validators
{
    public class PermissionRequestValidator : AbstractValidator<List<PermissionRequest>>
    {
        public PermissionRequestValidator(IOAuth2DbContext pContext)
        {
            RuleForEach(x => x)
            .ChildRules(permissionRequest =>
            {
                permissionRequest.RuleFor(x => x.Name)
                    .NotEmpty()
                    .WithMessage(ValidatorTransform.Required(Modules.Permission.Name))
                    .MinimumLength(Modules.UserNameMin)
                    .MustAsync(async (name, token) =>
                    {
                        var exists = await pContext.Permissions.FirstOrDefaultAsync(x => x.Name == name);
                        return exists == null;
                    })
                    .WithMessage(name => ValidatorTransform.Exists(Modules.Permission.Name));
            });
        }
    }
}
