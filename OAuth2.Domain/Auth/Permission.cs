namespace OAuth2.Domain.Auth
{
    public class Permission
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public List<UserPermission> UserPermissions { get; set; }

        public List<RolePermission> RolePermissions { get; set; }
    }
}
