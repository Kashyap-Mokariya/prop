using FluentValidation;
using PropVivo.Application.Common.Validation;
using PropVivo.Application.Constants;

namespace PropVivo.Application.Dto.RoleFeature.CreateRole
{
    public class CreateRoleValidator : AbstractValidator<CreateRoleRequest>
    {
        public CreateRoleValidator()
        {
            AddRuleForRequest();
            AddRuleForName();
            AddRuleForLegalEntitySubType();
            AddRuleForBusinessType();
            AddRuleForCountry();
            AddRuleForLegalEntityType();
            AddRuleForExecutionRequest();
        }

        private void AddRuleForBusinessType()
        {
            RuleFor(x => x.BusinessTypeId)
                .NotNull()
                .NotEmpty()
                .WithMessage("Business Type Id is required.");

            RuleFor(x => x.BusinessTypeName)
                .NotNull()
                .NotEmpty()
                .WithMessage("Business Type Name is required.");
        }

        private void AddRuleForCountry()
        {
            RuleFor(x => x.BusinessTypeId)
                .NotNull()
                .NotEmpty()
                .WithMessage("Country Id is required.");

            RuleFor(x => x.BusinessTypeName)
                .NotNull()
                .NotEmpty()
                .WithMessage("Country Name is required.");
        }

        private void AddRuleForExecutionRequest()
        {
            // Validate ExecutionContext if it is provided
            RuleFor(x => x.ExecutionContext).NotNull()
           .NotEmpty().WithMessage("Execution context is required.")
                .SetValidator(new ExecutionContextValidator())
                .When(x => x.ExecutionContext != null);
        }

        private void AddRuleForLegalEntitySubType()
        {
            RuleFor(x => x.LegalEntitySubTypes)
                .NotNull()
                .WithMessage("Legal Enity Sub Type is required.")
                .Must(list => list != null && list.Count > 0).WithMessage("LegalEntitySubTypes must contain at least one item.")
                .ForEach(subType => subType.SetValidator(new LegalEntitySubTypeValidator()));
        }

        private void AddRuleForLegalEntityType()
        {
            RuleFor(x => x.LegalEntityTypeId)
                .NotNull()
                .NotEmpty()
                .WithMessage("Legal Entity Type Id is required.");

            RuleFor(x => x.LegalEntityTypeName)
                .NotNull()
                .NotEmpty()
                .WithMessage("Legal Entity Type Name is required.");
        }

        private void AddRuleForName()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage("Name is required.")
                .MaximumLength(MaxLengths.Role.Name)
                .WithMessage($"Role Name may not be longer than {MaxLengths.Role.Name} characters"); ;
        }

        private void AddRuleForRequest()
        {
            RuleFor(x => x)
                .NotNull()
                .NotEmpty()
                .WithMessage(string.Format(Messaging.InvalidRequest));
        }
    }

    public class LegalEntitySubTypeValidator : AbstractValidator<LegalEntitySubType>
    {
        public LegalEntitySubTypeValidator()
        {
            RuleFor(x => x.LegalEntitySubTypeId)
                .NotEmpty().WithMessage("Legal Entity SubType Id is required.");

            RuleFor(x => x.LegalEntitySubTypeName)
                .NotEmpty().WithMessage("Legal Entity SubType Name is required.");
        }
    }
}