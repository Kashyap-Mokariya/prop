using PropVivo.Application.Dto.CallFeature.IncomingCall;

namespace PropVivo.Application.Services
{
    public interface ICallNotificationService
    {
        Task NotifyIncomingCallAsync(IncomingCallResponse callData);
        Task NotifyCallStatusUpdateAsync(string callId, string status);
    }
}
