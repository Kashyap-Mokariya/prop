namespace PropVivo.Application.Common.Base
{
    public class BaseResponsePagination<T> : BaseReponseGeneric<T>
    {
        public int Count { get; set; }
        public Meta? Meta { get; set; }
    }

    public class Meta
    {
        public bool HasNextPage => PageNumber < TotalPages;
        public bool HasPreviousPage => PageNumber > 1;
        public int PageNumber => (Skip / Take) + 1;
        public int Skip { get; set; }
        public int Take { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / Take);
    }
}