using Microsoft.AspNetCore.SignalR;

namespace PropVivo.API.Hubs
{
    public class CallHub : Hub
    {
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task SendCallUpdate(string callId, object callData)
        {
            await Clients.All.SendAsync("CallUpdate", callId, callData);
        }

        public async Task SendVoiceData(string callId, string audioData)
        {
            await Clients.Others.SendAsync("ReceiveVoiceData", callId, audioData);
        }
    }
}
