using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Common.Exceptions;
using PropVivo.Application.Constants;
using PropVivo.Application.Repositories;
using PropVivo.Domain.Entities.Call;
using System.Net;

namespace PropVivo.Application.Dto.CallFeature.IncomingCall
{
    public sealed class IncomingCallHandler : IRequestHandler<IncomingCallRequest, BaseResponse<IncomingCallResponse>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICallRepository _callRepository;
        private readonly ClaimsPrincipalExtensions _userInfo;

        public IncomingCallHandler(
            ICustomerRepository customerRepository, 
            ICallRepository callRepository,
            ClaimsPrincipalExtensions userInfo)
        {
            _customerRepository = customerRepository;
            _callRepository = callRepository;
            _userInfo = userInfo;
        }

        public async Task<BaseResponse<IncomingCallResponse>> Handle(IncomingCallRequest request, CancellationToken cancellationToken)
        {
            if (request == null || string.IsNullOrEmpty(request.CallerPhoneNumber))
                throw new BadRequestException(string.Format(Messaging.InvalidRequest));

            var response = new BaseResponse<IncomingCallResponse>();

            // Look up customer by phone number
            var customer = await _customerRepository.GetCustomerByPhoneNumberAsync(request.CallerPhoneNumber);

            // Create call record
            var call = new Call
            {
                Id = string.IsNullOrEmpty(request.CallId) ? Guid.NewGuid().ToString() : request.CallId,
                CustomerId = customer?.Id,
                CallerPhoneNumber = request.CallerPhoneNumber,
                ReceiverPhoneNumber = request.ReceiverPhoneNumber,
                CallStatus = CallStatus.Incoming,
                StartTime = request.CallTime,
                UserContext = _userInfo.GetUserBaseContext(DateTime.UtcNow)
            };

            await _callRepository.AddItemAsync(call, nameof(Call));

            response.Data = new IncomingCallResponse
            {
                CallId = call.Id,
                Customer = customer,
                CallerPhoneNumber = request.CallerPhoneNumber,
                CustomerFound = customer != null,
                CallTime = request.CallTime
            };

            response.Success = true;
            response.StatusCode = (int)HttpStatusCode.OK;
            response.Message = "Incoming call processed successfully";

            return response;
        }
    }
}
