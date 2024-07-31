using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OAuth2.Domain.Request;
using OAuth2.Infrastructure.Transforms;
using OAuth2.Persistence;

namespace OAuth2.Service.Validators
{
    public class AssignPermissionRequestValidator : AbstractValidator<RevokeOrAssignPermissionRequest>
    {
        public AssignPermissionRequestValidator(IOAuth2DbContext pContext)
        {
            RuleFor(x => x.UserId)
                .MustAsync(async (id, token) =>
                {
                    var exists = await pContext.Users.FindAsync(id);
                    return exists != null;
                })
                .WithMessage(id => ValidatorTransform.NotExistsValueInTable(Modules.UserRole.UserId, Modules.User.Module));

            RuleFor(x => x.Permissions)
                .Must(permissionsId => permissionsId != null && permissionsId.Any())
                .WithMessage(ValidatorTransform.Required(Modules.UserPermission.PermissionId))
                .ForEach(permissionId =>
                {
                    permissionId.MustAsync(async (name, token) =>
                    {
                        var hasPermission = await pContext.Permissions.AnyAsync(x => x.Name == name);
                        return hasPermission;
                    })
                    .WithMessage(id => ValidatorTransform.NotExistsValueInTable(Modules.UserPermission.PermissionId, Modules.Permission.Module));
                });
        }
    }
}
