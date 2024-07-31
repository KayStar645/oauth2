namespace OAuth2.Domain.Request
{
    public class CreateRoleRequest
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public List<string>? PermissionsName { get; set; }
    }
}
