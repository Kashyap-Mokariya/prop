using Microsoft.AspNetCore.SignalR;
using PropVivo.API.Hubs;
using PropVivo.Application.Dto.CallFeature.IncomingCall;
using PropVivo.Application.Services;

namespace PropVivo.Infrastructure.Services
{
    public class CallNotificationService : ICallNotificationService
    {
        private readonly IHubContext<CallHub> _hubContext;

        public CallNotificationService(IHubContext<CallHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyIncomingCallAsync(IncomingCallResponse callData)
        {
            await _hubContext.Clients.All.SendAsync("IncomingCall", callData);
        }

        public async Task NotifyCallStatusUpdateAsync(string callId, string status)
        {
            await _hubContext.Clients.All.SendAsync("CallStatusUpdate", new { callId, status });
        }
    }
}
