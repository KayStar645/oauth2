using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OAuth2.Domain.Auth;
using OAuth2.Domain.Common;
using OAuth2.Domain.Request;
using OAuth2.Domain.Responses;
using OAuth2.Persistence;
using OAuth2.Service.Contract;
using OAuth2.Service.Validators;

namespace OAuth2.Service.Implementation
{
    public class PermissionService : IPermissionService
    {
        private readonly IOAuth2DbContext _context;
        private readonly IMapper _mapper;

        public PermissionService(IOAuth2DbContext pContext, IMapper pMapper)
        {
            _context = pContext;
            _mapper = pMapper;
        }

        public async Task<Result<List<PermissionResponses>>> Create(List<PermissionRequest> pRequest)
        {
            var validator = new PermissionRequestValidator(_context);
            var validationResult = await validator.ValidateAsync(pRequest);

            if (validationResult.IsValid == false)
            {
                var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return Result<List<PermissionResponses>>.Failure(errorMessages, StatusCodes.Status400BadRequest);
            }

            try
            {
                List<PermissionResponses> result = new List<PermissionResponses>();

                foreach (var permission in pRequest)
                {
                    var per = await _context.Permissions.FirstOrDefaultAsync(x => x.Name == permission.Name);

                    if (per == null)
                    {
                        var newPer = _mapper.Map<Permission>(permission);
                        var newPermission = await _context.Permissions.AddAsync(newPer);
                        await _context.SaveChangesAsync();
                        result.Add(_mapper.Map<PermissionResponses>(newPermission.Entity));
                    }
                }

                return Result<List<PermissionResponses>>.Success(result, StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return Result<List<PermissionResponses>>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<Result<List<PermissionResponses>>> Get()
        {
            var permissions = await _context.Permissions.ToListAsync();

            var result = _mapper.Map<List<PermissionResponses>>(permissions);

            return Result<List<PermissionResponses>>.Success(result, StatusCodes.Status200OK);
        }
    }
}
