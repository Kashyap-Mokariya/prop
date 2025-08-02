using PropVivo.Application.Common.Base;

namespace PropVivo.Application.Common
{
    public class LegalEntityDto : ExecutionRequest
    {
        public string? Code { get; set; }

        public string? Domain { get; set; }

        public string? LegalEntityId { get; set; }

        public string? Name { get; set; }

        public string? Website { get; set; }
    }
}