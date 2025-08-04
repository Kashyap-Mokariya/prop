using PropVivo.Domain.Entities.Customer;

namespace PropVivo.Application.Dto.CallFeature.IncomingCall
{
    public class IncomingCallResponse
    {
        public string? CallId { get; set; }
        public Customer? Customer { get; set; }
        public string? CallerPhoneNumber { get; set; }
        public bool CustomerFound { get; set; }
        public DateTime CallTime { get; set; }
    }
}
