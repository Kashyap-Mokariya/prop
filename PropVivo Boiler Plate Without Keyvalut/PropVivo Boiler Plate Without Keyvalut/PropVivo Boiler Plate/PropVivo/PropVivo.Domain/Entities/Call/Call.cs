using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PropVivo.Domain.Common;
using PropVivo.Domain.Enums;

namespace PropVivo.Domain.Entities.Call
{
    public enum CallStatus
    {
        Incoming,
        Connected,
        OnHold,
        Ended,
        Missed
    }

    public class Call : BaseEntity
    {
        public string? CustomerId { get; set; }
        public string? CallerPhoneNumber { get; set; }
        public string? ReceiverPhoneNumber { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public CallStatus CallStatus { get; set; } = CallStatus.Incoming;
        
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int Duration { get; set; } // in seconds
        public string? Notes { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public Status Status { get; set; } = Status.Active;
        
        public UserBase? UserContext { get; set; }
    }
}
