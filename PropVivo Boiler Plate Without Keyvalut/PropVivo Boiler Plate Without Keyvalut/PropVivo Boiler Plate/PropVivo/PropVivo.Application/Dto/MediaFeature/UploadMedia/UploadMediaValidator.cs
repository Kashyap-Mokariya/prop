using FluentValidation;
using Microsoft.AspNetCore.Http;
using PropVivo.Application.Common.Validation;
using PropVivo.Application.Constants;

namespace PropVivo.Application.Dto.MediaFeature.UploadMedia
{
    public class UploadMediaValidator : AbstractValidator<UploadMediaRequest>
    {
        public UploadMediaValidator()
        {
            AddRuleForFile();
            AddRuleForRequest();
            AddRuleForContainerName();
            AddRuleForExecutionRequest();
        }

        private void AddRuleForContainerName()
        {
            RuleFor(x => x.ContainerName)
           .NotEmpty().WithMessage("Container name is required.")
           .Length(3, 63).WithMessage("Storage container name must be between 3 and 63 characters.")
           .Matches(@"^[a-z0-9-]+$").WithMessage("Storage container name can only contain lowercase letters, numbers, and hyphens.")
           .Must(BeValidContainerName).WithMessage("Storage container name must start with a letter or number and cannot contain consecutive hyphens.");
        }

        private void AddRuleForExecutionRequest()
        {
            // Validate ExecutionContext if it is provided
            RuleFor(x => x.ExecutionContext).NotNull()
           .NotEmpty().WithMessage("Execution context is required.")
                .SetValidator(new ExecutionContextValidator())
                .When(x => x.ExecutionContext != null);
        }

        private void AddRuleForFile()
        {
            RuleFor(x => x.FormFile)
            .NotNull()
            .WithMessage("Blob file is required.")
            .Must(BeValidFileName).WithMessage("Blob file name contains invalid characters.")
            .Must(file => file.Length > 0).WithMessage("Blob file length must be greater than zero.");
        }

        private void AddRuleForRequest()
        {
            RuleFor(x => x)
                .NotNull()
                .NotEmpty()
                .WithMessage(string.Format(Messaging.InvalidRequest));
        }

        private bool BeValidContainerName(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
                return false;

            return !containerName.StartsWith('-') && !containerName.EndsWith('-') && !containerName.Contains("--");
        }

        private bool BeValidFileName(IFormFile file)
        {
            if (file == null) return false;

            char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
            string fileName = file.FileName;

            // Ensure file name does not contain invalid characters and is not empty
            return !string.IsNullOrWhiteSpace(fileName) && fileName.All(c => !invalidFileNameChars.Contains(c));
        }
    }
}