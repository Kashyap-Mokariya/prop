using FluentValidation;
using PropVivo.Application.Common.Validation;
using PropVivo.Application.Constants;

namespace PropVivo.Application.Dto.MediaFeature.DownloadMedia
{
    public class DownloadMediaValidator : AbstractValidator<DownloadMediaRequest>
    {
        public DownloadMediaValidator()
        {
            AddRuleForRequest();
            AddRuleForContainerName();
            AddRuleForFilePath();
            AddRuleForExecutionRequest();
        }

        private void AddRuleForContainerName()
        {
            RuleFor(x => x.ContainerName).NotNull()
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
                .SetValidator(new ExecutionContextValidator());
        }

        private void AddRuleForFilePath()
        {
            RuleFor(x => x.FilePath)
            .NotNull()
            .NotEmpty()
            .WithMessage("File path is required.")
            .Must(BeValidFileName).WithMessage("File path contains invalid characters.");
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

        private bool BeValidFileName(string filePath)
        {
            char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
            string fileName = filePath;

            // Ensure file name does not contain invalid characters and is not empty
            return !string.IsNullOrWhiteSpace(fileName) && fileName.All(c => !invalidFileNameChars.Contains(c));
        }
    }
}