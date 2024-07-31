using OAuth2.Domain.Common;
using OAuth2.Domain.Request;
using OAuth2.Domain.Responses;

namespace OAuth2.Service.Contract
{
    public interface IAccountService
    {
        /*
        - Tạo tài khoản - 
        - Tạo vai trò - 
        - Cập nhật vai trò và gán vai trò cho người dùng (nhiều vai trò - 1 người dùng)
        - Tạo quyền (nhiều quyền)
        - Cập nhật quyền cho vài trò (nhiều quyền - 1 vai trò)
        - Gán quyền cho người dùng (1 quyền - 1 người dùng)
        - Thu hồi quyền của người dùng (1 quyền - 1 người dùng)
        - Đăng nhập với gg/fb
        - Quên mật khẩu
        - Đổi mật khẩu - Có thông báo tới mail/phone
         */
        Task<Result<RegistrationResponses>> RegisterAsync(RegisterRequest pRequest);

        Task<Result<LoginResponses>> LoginAsync(LoginRequest pRequest);

        Task<Result<LoginResponses>> GoogleLoginAsync(GoogleLoginRequest pRequest);
    }
}
