namespace AdHr.Repository.Models
{
    public class RepositoryResponse<T>
    {
        public RepositoryResponse()
        {
            HasSuccess = false;
            NotFound = false;
            Message = string.Empty;
        }

        public RepositoryResponse(T data) : this()
        {
            Data = data;
        }

        public bool HasSuccess { get; set; }
        public bool NotFound { get; set; }
        public string Message { get; set; }

        public T Data { get; set; }
    }
}
