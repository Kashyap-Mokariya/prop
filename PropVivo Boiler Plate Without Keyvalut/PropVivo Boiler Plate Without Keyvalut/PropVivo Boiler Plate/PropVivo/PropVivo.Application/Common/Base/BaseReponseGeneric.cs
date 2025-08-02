using Newtonsoft.Json;

namespace PropVivo.Application.Common.Base
{
    public class BaseReponseGeneric<T>
    {
        [JsonProperty("data")]
        public T? Data { get; set; }

        [JsonProperty("message")]
        public string? Message { get; set; }

        [JsonProperty("statuscode")]
        public int StatusCode { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }
    }
}