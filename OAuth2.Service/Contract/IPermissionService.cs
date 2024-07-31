using OAuth2.Domain.Common;
using OAuth2.Domain.Request;
using OAuth2.Domain.Responses;

namespace OAuth2.Service.Contract
{
    public interface IPermissionService
    {
        /*
            - Lấy danh sách permission - Mục đích hỗ trợ lập trình
            - Tạo nhiều permission - Mục đích hỗ trợ lập trình
         */

        Task<Result<List<PermissionResponses>>> Create(List<PermissionRequest> pPermissions);

        Task<Result<List<PermissionResponses>>> Get();
    }
}
