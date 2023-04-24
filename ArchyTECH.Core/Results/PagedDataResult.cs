namespace ArchyTECH.Core.Results
{
    public class PagedDataResult<T>: DataResult<List<T>>
    {
        public PagedDataResult()
        {
            Success = true;
        }

        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public int Total { get; set; }

    }
}