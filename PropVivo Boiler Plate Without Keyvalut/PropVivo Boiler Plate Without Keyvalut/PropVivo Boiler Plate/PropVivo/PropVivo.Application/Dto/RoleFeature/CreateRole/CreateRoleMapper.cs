using AutoMapper;
using PropVivo.Application.Common.Mapper;
using PropVivo.Domain.Entities.FeatureRolePermissionMaster;
using PropVivo.Domain.Entities.FeatureRolePermissionMaster.SupportingTypes;

namespace PropVivo.Application.Dto.RoleFeature.CreateRole
{
    public sealed class CreateRoleMapper : Profile
    {
        public CreateRoleMapper()
        {
            CreateMap<CreateRoleRequest, Role>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.UserContext, opt => opt.MapFrom<UserContextValueResolver<CreateRoleRequest, Role>>())
                .AfterMap((source, destination) =>
                {
                    destination.SetCustomDocumentType(nameof(FeatureRolePermissionMaster));
                    destination.Type = nameof(Role);
                });
        }
    }
}