using OAuth2.Domain.Common.Interfaces;

namespace OAuth2.Domain.Common
{
    public class Result<T> : IResult<T>
    {

        public T Data { get; set; }
        public List<string> Messages { get; set; } = new List<string>();

        public Exception Exception { get; set; }

        public bool Succeeded { get; set; }

        public int Code { get; set; }

        #region Non Async Methods 

        #region Success Methods 

        public static Result<T> Success()
        {
            return new Result<T>
            {
                Succeeded = true,
                Code = 200,
            };
        }

        public static Result<T> Success(string message, int code)
        {
            return new Result<T>
            {
                Succeeded = true,
                Messages = new List<string> { message },
                Code = code
            };
        }

        public static Result<T> Success(T data, int code)
        {
            return new Result<T>
            {
                Succeeded = true,
                Data = data,
                Code = code
            };
        }

        public static Result<T> Success(T data, string message, int code)
        {
            return new Result<T>
            {
                Succeeded = true,
                Messages = new List<string> { message },
                Data = data,
                Code = code
            };
        }

        #endregion

        #region Failure Methods 

        public static Result<T> Failure()
        {
            return new Result<T>
            {
                Succeeded = false,
                Code = 404,
            };
        }

        public static Result<T> Failure(string message, int code)
        {
            return new Result<T>
            {
                Succeeded = false,
                Messages = new List<string> { message },
                Code = code,
            };
        }

        public static Result<T> Failure(List<string> messages, int code)
        {
            return new Result<T>
            {
                Succeeded = false,
                Messages = messages,
                Code = code,
            };
        }

        public static Result<T> Failure(T data, int code)
        {
            return new Result<T>
            {
                Succeeded = false,
                Data = data,
                Code = code,
            };
        }

        public static Result<T> Failure(T data, string message, int code)
        {
            return new Result<T>
            {
                Succeeded = false,
                Messages = new List<string> { message },
                Data = data,
                Code = code,
            };
        }

        public static Result<T> Failure(T data, List<string> messages, int code)
        {
            return new Result<T>
            {
                Succeeded = false,
                Messages = messages,
                Data = data,
                Code = code,
            };
        }

        public static Result<T> Failure(Exception exception, int code)
        {
            return new Result<T>
            {
                Succeeded = false,
                Exception = exception,
                Code = code,
            };
        }

        #endregion

        #endregion

        #region Async Methods 

        #region Success Methods 

        public static Task<Result<T>> SuccessAsync()
        {
            return Task.FromResult(Success());
        }

        public static Task<Result<T>> SuccessAsync(string message, int code)
        {
            return Task.FromResult(Success(message, code));
        }

        public static Task<Result<T>> SuccessAsync(T data, int code)
        {
            return Task.FromResult(Success(data, code));
        }

        public static Task<Result<T>> SuccessAsync(T data, string message, int code)
        {
            return Task.FromResult(Success(data, message, code));
        }

        #endregion

        #region Failure Methods 

        public static Task<Result<T>> FailureAsync()
        {
            return Task.FromResult(Failure());
        }

        public static Task<Result<T>> FailureAsync(string message, int code)
        {
            return Task.FromResult(Failure(message, code));
        }

        public static Task<Result<T>> FailureAsync(List<string> messages, int code)
        {
            return Task.FromResult(Failure(messages, code));
        }

        public static Task<Result<T>> FailureAsync(T data, int code)
        {
            return Task.FromResult(Failure(data, code));
        }

        public static Task<Result<T>> FailureAsync(T data, string message, int code)
        {
            return Task.FromResult(Failure(data, message, code));
        }

        public static Task<Result<T>> FailureAsync(T data, List<string> messages, int code)
        {
            return Task.FromResult(Failure(data, messages, code));
        }

        public static Task<Result<T>> FailureAsync(Exception exception)
        {
            return Task.FromResult(Failure(exception, 500));
        }

        #endregion

        #endregion
    }
}
