using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PropVivo.Domain.Enums;

namespace PropVivo.Domain.Common
{
    public class Media
    {
        public string? FileExtension { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public long? FileSize { get; set; }
        public string? MediaId { get; set; }
        public string? MediaType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Status Status { get; set; } = Status.Active;

        public UserBase? UserContext { get; set; }
    }
}