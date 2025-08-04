using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PropVivo.Domain.Common;
using PropVivo.Domain.Enums;

namespace PropVivo.Domain.Entities.Customer
{
    public class Customer : BaseEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Company { get; set; }
        public Address? Address { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public Status Status { get; set; } = Status.Active;
        
        public UserBase? UserContext { get; set; }
        
        public string FullName => $"{FirstName} {LastName}".Trim();
    }
}
