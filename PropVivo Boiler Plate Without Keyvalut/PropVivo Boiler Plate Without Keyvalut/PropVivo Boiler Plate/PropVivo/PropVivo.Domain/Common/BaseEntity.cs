using Newtonsoft.Json;

namespace PropVivo.Domain.Common
{
    public abstract class BaseEntity : DocumentBase
    {
        [JsonProperty(PropertyName = "id")]
        public virtual string Id { get; set; }
    }
}