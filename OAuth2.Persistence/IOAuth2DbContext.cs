using Microsoft.EntityFrameworkCore;
using OAuth2.Domain.Auth;

namespace OAuth2.Persistence
{
    public interface IOAuth2DbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<Permission> Permissions { get; set; }
        DbSet<UserRole> UserRoles { get; set; }
        DbSet<RolePermission> RolePermissions { get; set; }
        DbSet<UserPermission> UserPermissions { get; set; }

        Task<int> SaveChangesAsync();
    }
}
