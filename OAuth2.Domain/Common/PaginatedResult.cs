using System.Net;

namespace OAuth2.Domain.Common
{
    public class PaginatedResult<T> : Result<T>
    {
        public Extra Extra { get; set; }

        public PaginatedResult(bool succeeded, int code, T data = default, List<string> messages = null,
            int count = 0, int? currentPage = 1, int? pageSize = 10)
        {
            Data = data;
            Succeeded = succeeded;
            Messages = messages;
            Code = code;
            Extra = new Extra(count, currentPage, pageSize);
        }

        public static PaginatedResult<T> Create(T data, int count, int? pageNumber, int? pageSize, int code)
        {
            return new PaginatedResult<T>(true, code, data, null, count, pageNumber, pageSize);
        }

        public static PaginatedResult<T> Success(T data, int count, int? pageNumber, int? pageSize)
        {
            return new PaginatedResult<T>(true, (int)HttpStatusCode.OK, data, null, count, pageNumber, pageSize);
        }

        public static PaginatedResult<T> Failure(int code, List<string> messages)
        {
            return new PaginatedResult<T>(false, code, default, messages, 0, 1, 10);
        }
    }
}