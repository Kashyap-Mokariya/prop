using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using PropVivo.API.Hubs;
using PropVivo.Application.Dto.CallFeature.IncomingCall;
using PropVivo.Application.Services;

namespace PropVivo.Infrastructure.Services
{
    public class CallNotificationService : ICallNotificationService
    {
        private readonly IHubContext<CallHub> _hub;

        public CallNotificationService(IHubContext<CallHub> hub) => _hub = hub;

        public Task NotifyIncomingCallAsync(IncomingCallResponse data) =>
            _hub.Clients.All.SendAsync("IncomingCall", data);

        public Task NotifyCallStatusUpdateAsync(string callId, string status) =>
            _hub.Clients.All.SendAsync("CallStatusUpdate", callId, status);
    }
}
