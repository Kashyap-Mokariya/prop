using System;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;

namespace PropVivo.Application.Services
{
    public interface ITwilioService
    {
        Task<CallResource> MakeCallAsync(string to, string from, Uri statusCallback);
        Task<CallResource> GetCallDetailsAsync(string callSid);
    }
}
