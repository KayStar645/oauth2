namespace OAuth2.Domain.Responses
{
    public class RoleResponses
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public List<string>? PermissionsName { get; set; }
    }
}
