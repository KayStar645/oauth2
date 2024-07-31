using OAuth2.Domain.Common;
using OAuth2.Domain.Request;
using OAuth2.Domain.Responses;

namespace OAuth2.Service.Contract
{
    public interface IRoleService
    {
        Task<Result<RoleResponses>> CreateAsync(CreateRoleRequest pRequest);

        Task<Result<RoleResponses>> UpdateAsync(UpdateRoleRequest pRequest);

        Task<Result<List<string>>> AssignRoles(AssignRoleRequest pRequest);
    }
}
