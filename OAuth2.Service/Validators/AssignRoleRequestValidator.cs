using FluentValidation;
using OAuth2.Domain.Request;
using OAuth2.Infrastructure.Transforms;
using OAuth2.Persistence;

namespace OAuth2.Service.Validators
{
    public class AssignRoleRequestValidator : AbstractValidator<AssignRoleRequest>
    {
        public AssignRoleRequestValidator(IOAuth2DbContext pContext)
        {
            RuleFor(x => x.UserId)
                .MustAsync(async (id, token) =>
                {
                    var exists = await pContext.Users.FindAsync(id);
                    return exists != null;
                })
                .WithMessage(id => ValidatorTransform.NotExistsValueInTable(Modules.UserRole.UserId, Modules.User.Module));

            RuleFor(x => x.RolesId)
                .Must(rolesId => rolesId != null && rolesId.Any())
                .WithMessage(ValidatorTransform.Required(Modules.UserRole.RoleId))
                .ForEach(roleId =>
                {
                    roleId.MustAsync(async (id, token) =>
                    {
                        var exists = await pContext.Roles.FindAsync(id);
                        return exists != null;
                    })
                    .WithMessage(id => ValidatorTransform.NotExistsValueInTable(Modules.UserRole.RoleId, Modules.Role.Module));
                });
        }
    }
}
