using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Dto.CallFeature.IncomingCall;
using Microsoft.AspNetCore.Authorization;

namespace PropVivo.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CallController : BaseController
    {
        private readonly IMediator _mediator;

        private readonly ICallNotificationService _notificationService;
        private readonly IVoiceModulationService _voiceModulationService;

        public CallController(
            IMediator mediator,
            ICallNotificationService notificationService,
            IVoiceModulationService voiceModulationService
        )
        {
            _mediator = mediator;
            _notificationService = notificationService;
            _voiceModulationService = voiceModulationService;
        }

        public CallController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("incoming")]
        [ProducesDefaultResponseType(typeof(BaseResponse<IncomingCallResponse>))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IncomingCallResponse>> HandleIncomingCallAsync(
            [FromBody] IncomingCallRequest request, 
            CancellationToken cancellationToken)
        {
            var resp = await _mediator.Send(request, cancellationToken);
            await _notificationService.NotifyIncomingCallAsync(resp.Data);
        }

        [HttpPost("webhook")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("webhook/incoming")]
        public async Task<IActionResult> ReceiveCallWebhook([FromForm] TwilioWebhookRequest data)
        {
            if (string.IsNullOrEmpty(data.From))
                return BadRequest("Invalid Twilio webhook");

            var request = new IncomingCallRequest
            {
                CallerPhoneNumber = data.From,
                ReceiverPhoneNumber = data.To,
                CallId = data.CallSid,
                CallTime = DateTime.UtcNow,
                ExecutionContext = new ExecutionContext
                {
                    TrackingId = Guid.NewGuid().ToString(),
                    SessionId = "twilio-webhook",
                },
            };

            var resp = await _mediator.Send(request);
            await _notificationService.NotifyIncomingCallAsync(resp.Data!);

            // Return TwiML to Twilio
            var twiml = new Twilio.TwiML.VoiceResponse();
            twiml.Say("Please wait while we connect you.");

            return Content(twiml.ToString(), "application/xml");
        }

        [HttpPost("modulate-voice")]
        public async Task<IActionResult> ModulateVoice([FromBody] VoiceModulationRequest req)
        {
            var audio = req.AudioData.Select(b => (byte)b).ToArray();
            var modulated = await _voiceModulationService.ModulateVoiceAsync(
                audio,
                req.FromAccent,
                req.ToAccent
            );
            return File(modulated, "audio/webm");
        }

        public class VoiceModulationRequest
        {
            public List<int> AudioData { get; set; }
            public string FromAccent { get; set; }
            public string ToAccent { get; set; }
        }

        public class TwilioWebhookRequest
        {
            public string CallSid { get; set; } // from form field "CallSid"
            public string From { get; set; } // from form field "From"
            public string To { get; set; } // from form field "To"
        }
    }
}
