using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OAuth2.Domain.Auth;
using OAuth2.Domain.Common;
using OAuth2.Domain.Request;
using OAuth2.Persistence;
using OAuth2.Service.Contract;
using OAuth2.Service.Exceptions;
using OAuth2.Service.Validators;

namespace OAuth2.Service.Implementation
{
    public class UserService : IUserService
    {
        private readonly IOAuth2DbContext _context;

        public UserService(IOAuth2DbContext pContext)
        {
            _context = pContext;
        }

        public async Task<Result<List<string>>> AssignPermissions(RevokeOrAssignPermissionRequest pRequest)
        {
            var validator = new AssignPermissionRequestValidator(_context);
            var validationResult = await validator.ValidateAsync(pRequest);

            if (validationResult.IsValid == false)
            {
                var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return Result<List<string>>.Failure(errorMessages, StatusCodes.Status400BadRequest);
            }

            foreach(var permissionName in pRequest.Permissions)
            {
                var permission = await _context.Permissions.FirstOrDefaultAsync(x => x.Name == permissionName);

                var hasUP = await _context.UserPermissions
                                    .Include(x => x.Permission)
                                    .FirstOrDefaultAsync(x => x.UserId == pRequest.UserId &&
                                                   x.Permission.Name == permissionName);
                if(hasUP == null)
                {
                    await _context.UserPermissions.AddAsync(new UserPermission
                    {
                        UserId = pRequest.UserId,
                        PermissionId = permission.Id
                    });
                }    
            }    
            await _context.SaveChangesAsync();

            return Result<List<string>>.Success(pRequest.Permissions, StatusCodes.Status201Created);
        }

        public async Task RevokePermissions(RevokeOrAssignPermissionRequest pRequest)
        {
            var validator = new AssignPermissionRequestValidator(_context);
            var validationResult = await validator.ValidateAsync(pRequest);

            if (validationResult.IsValid == false)
            {
                var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                throw new BadRequestException(string.Join(", ", errorMessages));
            }

            foreach (var permissionName in pRequest.Permissions)
            {
                var hasUP = await _context.UserPermissions
                                    .Include(x => x.Permission)
                                    .FirstOrDefaultAsync(x => x.UserId == pRequest.UserId &&
                                                   x.Permission.Name == permissionName);
                if (hasUP != null)
                {
                    _context.UserPermissions.Remove(hasUP);
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
