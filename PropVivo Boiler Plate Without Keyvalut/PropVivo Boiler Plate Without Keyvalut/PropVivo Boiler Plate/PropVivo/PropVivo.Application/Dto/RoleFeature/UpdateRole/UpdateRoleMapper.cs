using AutoMapper;
using PropVivo.Application.Common.Mapper;
using PropVivo.Domain.Entities.FeatureRolePermissionMaster;
using PropVivo.Domain.Entities.FeatureRolePermissionMaster.SupportingTypes;

namespace PropVivo.Application.Dto.RoleFeature.UpdateRole
{
    public sealed class UpdateRoleMapper : Profile
    {
        public UpdateRoleMapper()
        {
            CreateMap<UpdateRoleRequest, Role>()
            .ForMember(dest => dest.UserContext, opt => opt.MapFrom<UserContextValueResolver<UpdateRoleRequest, Role>>()).AfterMap((source, destination) =>
            {
                destination.SetCustomDocumentType(nameof(FeatureRolePermissionMaster));
                destination.Type = nameof(Role);
            });
        }
    }
}