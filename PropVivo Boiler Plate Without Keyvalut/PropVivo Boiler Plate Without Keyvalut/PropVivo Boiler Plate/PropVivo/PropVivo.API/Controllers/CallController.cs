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
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        [HttpPost("webhook")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ReceiveCallWebhook([FromBody] dynamic webhookData)
        {
            // Extract phone number from webhook data (format depends on your call API provider)
            // Example for a generic webhook:
            var callerNumber = webhookData?.from?.ToString();
            var receiverNumber = webhookData?.to?.ToString();
            var callId = webhookData?.call_id?.ToString();

            if (string.IsNullOrEmpty(callerNumber))
                return BadRequest("Invalid webhook data");

            var request = new IncomingCallRequest
            {
                CallerPhoneNumber = callerNumber,
                ReceiverPhoneNumber = receiverNumber,
                CallId = callId,
                CallTime = DateTime.UtcNow,
                ExecutionContext = new PropVivo.Application.Common.Base.ExecutionContext
                {
                    TrackingId = Guid.NewGuid().ToString(),
                    SessionId = "webhook-session"
                }
            };

            var response = await _mediator.Send(request);
            return Ok(new { success = response.Success, callId = response.Data?.CallId });
        }
    }
}
