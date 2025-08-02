using FluentValidation;
using PropVivo.Application.Common.Validation;
using PropVivo.Application.Constants;
using PropVivo.Application.Dto.RoleFeature.GetAllRole;

namespace PropVivo.Application.Dto.RoleFeature.CreateRole
{
    public class GetAllRoleValidator : AbstractValidator<GetAllRoleRequest>
    {
        public GetAllRoleValidator()
        {
            AddRuleForRequest();
            AddRuleForPagination();
            AddRuleForExecutionRequest();
        }

        private void AddRuleForExecutionRequest()
        {
            // Validate ExecutionContext if it is provided
            RuleFor(x => x.ExecutionContext).NotNull()
           .NotEmpty().WithMessage("Execution context is required.")
                .SetValidator(new ExecutionContextValidator())
                .When(x => x.ExecutionContext != null);
        }

        private void AddRuleForPagination()
        {
            RuleFor(x => x.PageCriteria)
                .NotNull()
                .NotEmpty()
                .WithMessage("Pagination is required.");
        }

        private void AddRuleForRequest()
        {
            RuleFor(x => x)
                .NotNull()
                .NotEmpty()
                .WithMessage(string.Format(Messaging.InvalidRequest));
        }
    }
}