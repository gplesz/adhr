namespace AdHr.Models
{
    public class ResponseBase<T>
    {
        public ResponseBase()
        {
            HasSuccess = false;
            NotFound = false;
            Message = string.Empty;
        }

        public bool HasSuccess { get; set; }
        public bool NotFound { get; set; }
        public string Message { get; set; }

        public T Data { get; set; }
    }
}
