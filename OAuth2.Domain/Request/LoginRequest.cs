namespace OAuth2.Domain.Request
{
    public class LoginRequest
    {
        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Password { get; set; }

        public bool IsEmail { get; set; }
    }
}
