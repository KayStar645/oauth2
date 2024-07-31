namespace OAuth2.Infrastructure.Transforms
{
    public static class IdentityTransform
    {
        public static string UserNameAlreadyExists(string userName)
        {
            return $"Tên người dùng {userName} đã tồn tại!";
        }
        
        public static string EmailAlreadyExists(string userName)
        {
            return $"Email {userName} đã được đăng ký!";
        }

        public static string PhoneNumberAlreadyExists(string userName)
        {
            return $"Số điện thoại {userName} đã được đăng ký!";
        }

        public static string UserNotExists(string userName)
        {
            return $"Không tìm thấy người dùng {userName}!";
        }

        public static string InvalidCredentials(string userName)
        {
            return $"Thông tin xác thực của người dùng {userName} không hợp lệ!";
        }

        public static string ForbiddenException()
        {
            return "Bạn không được phép truy cập tài nguyên này!";
        }
    }
}
