using OAuth2.Domain.Common;
using OAuth2.Domain.Request;

namespace OAuth2.Service.Contract
{
    public interface IUserService
    {
        Task<Result<List<string>>> AssignPermissions(RevokeOrAssignPermissionRequest pRequest);

        Task RevokePermissions(RevokeOrAssignPermissionRequest pRequest);
    }
}
