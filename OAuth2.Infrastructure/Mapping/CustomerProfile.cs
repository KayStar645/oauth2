using AutoMapper;
using OAuth2.Domain.Auth;
using OAuth2.Domain.Request;
using OAuth2.Domain.Responses;

namespace OAuth2.Infrastructure.Mapping
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Role, RoleResponses>().ReverseMap();
            CreateMap<Role, CreateRoleRequest>().ReverseMap();
            CreateMap<Role, UpdateRoleRequest>().ReverseMap();

            CreateMap<Permission, PermissionRequest>().ReverseMap();
            CreateMap<Permission, PermissionResponses>().ReverseMap();
        }
    }
}
