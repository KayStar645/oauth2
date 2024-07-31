using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OAuth2.Domain.Auth;
using OAuth2.Domain.Common;
using OAuth2.Domain.Request;
using OAuth2.Domain.Responses;
using OAuth2.Persistence;
using OAuth2.Service.Contract;
using OAuth2.Service.Validators;
using System.Net;
using System.Transactions;

namespace OAuth2.Service.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly IOAuth2DbContext _context;

        private readonly IMapper _mapper;

        public RoleService(IOAuth2DbContext pContext, IMapper pMapper) 
        {
            _context = pContext;
            _mapper = pMapper;
        }

        public async Task<Result<List<string>>> AssignRoles(AssignRoleRequest pRequest)
        {
            var validator = new AssignRoleRequestValidator(_context);
            var validationResult = await validator.ValidateAsync(pRequest);

            if (validationResult.IsValid == false)
            {
                var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return Result<List<string>>.Failure(errorMessages, StatusCodes.Status400BadRequest);
            }


            var currentRoles = await _context.UserRoles
                                                .Where(x => x.UserId == pRequest.UserId)
                                                .Select(x => x.RoleId)
                                                .ToListAsync();

            var deleteRole = currentRoles.Cast<int>().Except(pRequest.RolesId).ToList();
            var addRole = pRequest.RolesId.Except(currentRoles.Cast<int>()).ToList();

            foreach (var role in deleteRole)
            {
                var result = await _context.UserRoles
                            .FirstOrDefaultAsync(x => x.UserId == pRequest.UserId && x.RoleId == role);
                _context.UserRoles.Remove(result);
            }

            foreach (var role in addRole)
            {
                await _context.UserRoles.AddAsync(new UserRole
                {
                    UserId = pRequest.UserId,
                    RoleId = role
                });
            }
            await _context.SaveChangesAsync();

            List<string> permission = new List<string>();
            foreach (var role in pRequest.RolesId)
            {
                var per = await _context.RolePermissions
                                           .Include(x => x.Permission)
                                           .Where(x => x.RoleId == role)
                                           .Select(x => x.Permission.Name)
                                           .ToListAsync();
                permission = permission.Union(per).ToList();
            }

            return Result<List<string>>.Success(permission, StatusCodes.Status201Created);
        }

        public async Task<Result<RoleResponses>> CreateAsync(CreateRoleRequest pRequest)
        {
            var validator = new CreateRoleRequestValidator(_context);
            var validationResult = await validator.ValidateAsync(pRequest);

            if (validationResult.IsValid == false)
            {
                var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return Result<RoleResponses>.Failure(errorMessages, StatusCodes.Status400BadRequest);
            }

            try
            {
                var role = _mapper.Map<Role>(pRequest);

                var newRole = await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync();
                List<string> permissions = new List<string>();

                if (pRequest.PermissionsName != null)
                {
                    foreach (string permissionName in pRequest.PermissionsName)
                    {
                        var permission = await _context.Permissions
                                    .FirstOrDefaultAsync(x => x.Name == permissionName);

                        if (permission != null)
                        {
                            var rolePermission = new RolePermission
                            {
                                RoleId = role.Id,
                                PermissionId = permission.Id
                            };
                            await _context.RolePermissions.AddAsync(rolePermission);
                            permissions.Add(permission.Name);
                        }
                    }
                    await _context.SaveChangesAsync();
                }

                var roleResponses = _mapper.Map<RoleResponses>(newRole.Entity);
                roleResponses.PermissionsName = permissions;
                return Result<RoleResponses>.Success(roleResponses, (int)HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Result<RoleResponses>.Failure(ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Result<RoleResponses>> UpdateAsync(UpdateRoleRequest pRequest)
        {
            var validator = new UpdateRoleRequestValidator(_context, pRequest.Id);
            var validationResult = await validator.ValidateAsync(pRequest);

            if (validationResult.IsValid == false)
            {
                var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return Result<RoleResponses>.Failure(errorMessages, StatusCodes.Status400BadRequest);
            }


            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var role = _mapper.Map<Role>(pRequest);
                    var updateRole = _context.Roles.Update(role);
                    await _context.SaveChangesAsync();
                    List<string> permissions = new List<string>();

                    // Get Permission hiện của của Role này
                    var currentPermissionsName = await _context.RolePermissions
                                                        .Include(x => x.Permission)
                                                        .Where(x => x.RoleId == pRequest.Id)
                                                        .Select(x => x.Permission.Name)
                                                        .ToListAsync();

                    if (pRequest.PermissionsName != null)
                    {
                        var deletePermission = currentPermissionsName.Except(pRequest.PermissionsName).ToList();
                        var addPermission = pRequest.PermissionsName.Except(currentPermissionsName).ToList();
                        var sharePermission = pRequest.PermissionsName.Intersect(currentPermissionsName).ToList();

                        foreach (string permissionName in deletePermission)
                        {
                            var delete = await _context.RolePermissions
                                        .Include(x => x.Permission)
                                        .Where(x => x.RoleId == role.Id && x.Permission.Name == permissionName)
                                        .FirstOrDefaultAsync();
                            if (delete != null)
                            {
                                _context.RolePermissions.Remove(delete);
                            }
                        }

                        foreach (string permissionName in addPermission)
                        {
                            var permission = await _context.Permissions
                                                 .FirstOrDefaultAsync(x => x.Name == permissionName);

                            if (permission != null)
                            {

                                var per = new RolePermission
                                {
                                    RoleId = role.Id,
                                    PermissionId = permission.Id
                                };
                                await _context.RolePermissions.AddAsync(per);
                                permissions.Add(permission.Name);
                            }
                        }
                        await _context.SaveChangesAsync();
                        permissions = permissions.Union(sharePermission).ToList();
                    }

                    transaction.Complete();

                    var roleResponses = _mapper.Map<RoleResponses>(updateRole.Entity);
                    roleResponses.PermissionsName = permissions;
                    return Result<RoleResponses>.Success(roleResponses, StatusCodes.Status200OK);
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return Result<RoleResponses>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
                }
            }
        }
    }
}
