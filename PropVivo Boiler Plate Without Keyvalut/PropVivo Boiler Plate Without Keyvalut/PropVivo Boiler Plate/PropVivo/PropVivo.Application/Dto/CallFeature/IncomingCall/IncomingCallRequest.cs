using MediatR;
using PropVivo.Application.Common.Base;

namespace PropVivo.Application.Dto.CallFeature.IncomingCall
{
    public class IncomingCallRequest : ExecutionRequest, IRequest<BaseResponse<IncomingCallResponse>>
    {
        public string? CallerPhoneNumber { get; set; }
        public string? ReceiverPhoneNumber { get; set; }
        public string? CallId { get; set; }
        public DateTime CallTime { get; set; } = DateTime.UtcNow;
    }
}
