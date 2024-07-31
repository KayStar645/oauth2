namespace OAuth2.Domain.Auth
{
    public class User
    {
        public int Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Password { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public List<UserRole> UserRoles { get; set; }

        public List<UserPermission> UserPermissions { get; set; }
    }
}
