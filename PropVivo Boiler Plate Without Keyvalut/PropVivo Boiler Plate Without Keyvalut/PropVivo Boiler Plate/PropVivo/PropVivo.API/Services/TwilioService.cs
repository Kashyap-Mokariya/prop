using Microsoft.Extensions.Configuration;
using PropVivo.Application.Services;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace PropVivo.API.Services
{
    public class TwilioService : ITwilioService
    {
        public TwilioService(IConfiguration config)
        {
            TwilioClient.Init(
                config["TwilioSettings:AccountSid"],
                config["TwilioSettings:AuthToken"]
            );
        }

        public Task<CallResource> MakeCallAsync(string to, string from, Uri statusCallback) =>
            CallResource.CreateAsync(
                to: new PhoneNumber(to),
                from: new PhoneNumber(from),
                statusCallback: statusCallback,
                statusCallbackMethod: Twilio.Http.HttpMethod.Post
            );

        public Task<CallResource> GetCallDetailsAsync(string callSid) =>
            CallResource.FetchAsync(callSid);
    }
}
