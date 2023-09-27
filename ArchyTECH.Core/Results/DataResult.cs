namespace ArchyTECH.Core.Results
{
    public class DataResult<T> : Result
    {
        public DataResult()
        {
            
        }
        public DataResult(bool success)
            : base(success)
        {
        }

        public DataResult(bool success, string message)
            : base(success, message)
        {
        }

        public T? Data { get; set; }
    }
}