using MediatR;
using PropVivo.Application.Common.Base;

namespace PropVivo.Application.Dto.RoleFeature.GetAllRole
{
    public class FeatureRolePermissionMasterRequest : Request
    {
        public string? BusinessTypeId { get; set; }
        public string? CountryId { get; set; }

        public bool? IsActive { get; set; }

        public string? LegalEntityTypeId { get; set; }

        public string? Name { get; set; }
        public string? SubTypeId { get; set; }
    }

    public class GetAllRoleRequest : FeatureRolePermissionMasterRequest, IRequest<BaseResponsePagination<GetAllRoleResponse>>
    {
        public string? RoleId { get; set; }
    }
}