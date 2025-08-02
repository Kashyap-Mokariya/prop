using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PropVivo.Domain.Common;
using PropVivo.Domain.Enums;

namespace PropVivo.Domain.Entities.ServiceRequest
{
    public class ServiceRequests : BaseEntity
    {
        public string? AssignedToUserId { get; set; }

        public string? AssignedToUserName { get; set; }

        public string? CaseCategoryId { get; set; }
        public string? CaseCategoryName { get; set; }
        public string? CaseId { get; set; }
        public string? CaseNumber { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public CaseOriginatingType? CaseOriginatingType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public CasePriority? CasePriority { get; set; }

        public string? CaseSubCategoryId { get; set; }
        public string? CaseSubCategoryName { get; set; }
        public string? CaseTopicId { get; set; }
        public string? CaseTopicName { get; set; }
        public string? Description { get; set; }
        public List<Media>? Documents { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public IsAuthorized? IsAuthorized { get; set; }

        public string? LastName { get; set; }
        public string? LegalEntityCode { get; set; }
        public string? LegalEntityId { get; set; }

        public string? LegalEntityName { get; set; }
        public string? Phone { get; set; }
        public string? PhoneofCaller { get; set; }
        public Address? PropertyAddress { get; set; }

        public string? PropertyId { get; set; }

        public int? RatingCount { get; set; } = 0;
        //public List<CaseSatisfaction>? Ratings { get; set; }

        //[JsonConverter(typeof(StringEnumConverter))]
        //public ServiceRequestStatus? ServiceRequestStatus { get; set; }

        public string? Title { get; set; }
        public string? UnitRoleId { get; set; }
        public string? UnitRoleName { get; set; }
        public UserBase? UserContext { get; set; }
        public string? UserName { get; set; }
        public string? UserProfileId { get; set; }
    }
}