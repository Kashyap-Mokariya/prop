using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PropVivo.Application.Common.Base
{
    public enum OperationExpression
    {
        Equals,
        NotEquals,
        Minor,
        MinorEquals,
        Mayor,
        MayorEquals,
        Like,
        Contains,
        NotContains,
        Any
    }

    public class ExecutionContext
    {
        public string? SessionId { get; set; }

        public string? TrackingId { get; set; }

        public Uri? Uri { get; set; }

        public string? UserId { get; set; }
    }

    public class ExecutionRequest
    {
        public ExecutionContext? ExecutionContext { get; set; }
    }

    public class FilterByCriteria
    {
        public string? Field { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public OperationExpression OperationExpression { get; set; }

        public object? Value { get; set; }
    }

    public class OrderByCriteria
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Order { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? OrderBy { get; set; }
    }

    public class PageCriteria
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool EnablePage { get; set; } = true;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool HasMoreData { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int PageSize { get; set; } = 10;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int Skip { get; set; }
    }

    public class Request : ExecutionRequest
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<FilterByCriteria>? FilterByCriteria { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public OrderByCriteria? OrderByCriteria { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PageCriteria? PageCriteria { get; set; }

        public List<FilterByCriteria>? GetFilterByCriteria(string[] filterByCriteriaValues)
        {
            if (filterByCriteriaValues != null && filterByCriteriaValues.Any())
                this.FilterByCriteria = filterByCriteriaValues.Select(json => JsonConvert.DeserializeObject<FilterByCriteria>(json)).ToList();

            return this.FilterByCriteria;
        }
    }
}