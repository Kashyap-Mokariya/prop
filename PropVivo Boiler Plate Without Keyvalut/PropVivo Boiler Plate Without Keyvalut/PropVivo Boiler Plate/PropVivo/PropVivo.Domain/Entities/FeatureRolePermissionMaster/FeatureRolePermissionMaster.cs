using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PropVivo.Domain.Common;
using PropVivo.Domain.Enums;

namespace PropVivo.Domain.Entities.FeatureRolePermissionMaster
{
    public class FeatureRolePermissionMaster : BaseEntity
    {
        public string? BusinessTypeId { get; set; }

        public string? BusinessTypeName { get; set; }

        public string? CountryId { get; set; }

        public string? CountryName { get; set; }

        public string? Description { get; set; }

        public bool IsMapped { get; set; } = false;
        public string? LegalEntitySubTypeId { get; set; }
        public string? LegalEntitySubTypeName { get; set; }
        public string? LegalEntityTypeId { get; set; }
        public string? LegalEntityTypeName { get; set; }
        public string? Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Status Status { get; set; } = Status.Active;

        public string? Type { get; set; }
        public UserBase? UserContext { get; set; }
    }
}