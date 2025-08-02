using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Dto.RoleFeature.CreateRole;
using PropVivo.Domain.Enums;

namespace PropVivo.Application.Dto.RoleFeature.UpdateRole
{
    public class UpdateRoleRequest : RoleDto, IRequest<BaseResponse<UpdateRoleResponse>>
    {
        public string? LegalEntitySubTypeId { get; set; }
        public string? LegalEntitySubTypeName { get; set; }
        public string? RoleId { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Status Status { get; set; } = Status.Active;
    }
}