using FluentValidation;

namespace PropVivo.Application.Common.Validation
{
    public class ExecutionContextValidator : AbstractValidator<Base.ExecutionContext>
    {
        public ExecutionContextValidator()
        {
            // SessionId: optional, but if provided, it should be a valid string
            RuleFor(x => x.SessionId)
                .NotEmpty().WithMessage("SessionId cannot be empty")
                .MaximumLength(50).WithMessage("SessionId cannot exceed 50 characters");

            // TrackingId: must be provided and a valid string
            RuleFor(x => x.TrackingId)
                .NotEmpty().WithMessage("TrackingId is required")
                .MaximumLength(50).WithMessage("TrackingId cannot exceed 50 characters");

            // Uri: optional, but if provided, it should be a valid URI
            RuleFor(x => x.Uri)
                 .NotEmpty().WithMessage("Uri cannot be empty")
                .Must(uri => uri == null || Uri.IsWellFormedUriString(uri.ToString(), UriKind.RelativeOrAbsolute))
                .WithMessage("Uri must be a valid URI");

            // UserId: optional, but if provided, it should be a valid string
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId cannot be empty")
                .MaximumLength(50).WithMessage("UserId cannot exceed 50 characters")
                .When(x => x.UserId != null);
        }
    }
}