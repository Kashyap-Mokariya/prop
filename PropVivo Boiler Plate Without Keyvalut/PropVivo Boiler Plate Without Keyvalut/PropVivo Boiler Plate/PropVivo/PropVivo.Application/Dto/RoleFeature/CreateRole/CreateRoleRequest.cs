using MediatR;
using PropVivo.Application.Common.Base;

namespace PropVivo.Application.Dto.RoleFeature.CreateRole
{
    public class CreateRoleRequest : RoleDto, IRequest<BaseResponse<CreateRoleResponse>>
    {
        public List<LegalEntitySubType>? LegalEntitySubTypes { get; set; }
    }

    public class FeatureRolePermissionMasterDto : ExecutionRequest
    {
        public string? BusinessTypeId { get; set; }

        public string? BusinessTypeName { get; set; }

        public string? CountryId { get; set; }

        public string? CountryName { get; set; }

        public string? Description { get; set; }

        public string? LegalEntityTypeId { get; set; }

        public string? LegalEntityTypeName { get; set; }

        public string? Name { get; set; }
    }

    public class LegalEntitySubType
    {
        public string? LegalEntitySubTypeId { get; set; }

        public string? LegalEntitySubTypeName { get; set; }
    }

    public class RoleDto : FeatureRolePermissionMasterDto
    {
    }
}