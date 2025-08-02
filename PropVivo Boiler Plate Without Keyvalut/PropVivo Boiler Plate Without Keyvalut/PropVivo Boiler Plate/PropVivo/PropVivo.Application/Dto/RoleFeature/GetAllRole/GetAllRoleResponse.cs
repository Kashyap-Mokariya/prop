using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PropVivo.Application.Dto.RoleFeature.CreateRole;
using PropVivo.Domain.Common;
using PropVivo.Domain.Enums;

namespace PropVivo.Application.Dto.RoleFeature.GetAllRole
{
    public class GetAllRoleResponse
    {
        public IEnumerable<RoleItem>? Roles { get; set; }
    }

    public class RoleItem : LegalEntitySubType
    {
        public bool IsMapped { get; set; }

        public string? RoleId { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Status Status { get; set; }

        public string? Type { get; set; }
        public UserBase? UserContext { get; set; }
    }
}