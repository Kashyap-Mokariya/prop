using FluentValidation;
using PropVivo.Application.Common.Validation;

namespace PropVivo.Application.Dto.RoleFeature.DeleteRole
{
    public class DeleteRoleValidator : AbstractValidator<DeleteRoleRequest>
    {
        public DeleteRoleValidator()
        {
            AddRuleForExecutionRequest();
            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage("Role Id is required.");
        }

        private void AddRuleForExecutionRequest()
        {
            // Validate ExecutionContext if it is provided
            RuleFor(x => x.ExecutionContext).NotNull()
           .NotEmpty().WithMessage("Execution context is required.")
                .SetValidator(new ExecutionContextValidator())
                .When(x => x.ExecutionContext != null);
        }
    }
}