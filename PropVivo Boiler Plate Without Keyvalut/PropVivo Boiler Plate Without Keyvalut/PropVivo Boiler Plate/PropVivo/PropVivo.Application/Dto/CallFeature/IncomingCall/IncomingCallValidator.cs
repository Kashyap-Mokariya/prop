using FluentValidation;
using PropVivo.Application.Common.Validation;
using PropVivo.Application.Constants;

namespace PropVivo.Application.Dto.CallFeature.IncomingCall
{
    public class IncomingCallValidator : AbstractValidator<IncomingCallRequest>
    {
        public IncomingCallValidator()
        {
            RuleFor(x => x)
                .NotNull()
                .WithMessage(string.Format(Messaging.InvalidRequest));

            RuleFor(x => x.CallerPhoneNumber)
                .NotEmpty()
                .WithMessage("Caller phone number is required.")
                .Matches(@"^\+?[1-9]\d{1,14}$")
                .WithMessage("Invalid phone number format.");

            RuleFor(x => x.ExecutionContext)
                .NotNull()
                .WithMessage("Execution context is required.")
                .SetValidator(new ExecutionContextValidator());
        }
    }
}
