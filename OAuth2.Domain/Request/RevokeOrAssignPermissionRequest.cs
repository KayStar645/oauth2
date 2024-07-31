namespace OAuth2.Domain.Request
{
    public class RevokeOrAssignPermissionRequest
    {
        public int UserId { get; set; }

        public List<string>? Permissions { get; set; }
    }
}
