using AutoMapper;
using PropVivo.Domain.Entities.FeatureRolePermissionMaster.SupportingTypes;

namespace PropVivo.Application.Dto.RoleFeature.GetAllRole
{
    public sealed class GetAllRoleMapper : Profile
    {
        public GetAllRoleMapper()
        {
            CreateMap<Role, RoleItem>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id));
        }
    }
}