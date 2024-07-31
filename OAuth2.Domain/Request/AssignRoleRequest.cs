namespace OAuth2.Domain.Request
{
    public class AssignRoleRequest
    {
        public int UserId { get; set; }

        public List<int>? RolesId { get; set; }
    }
}
