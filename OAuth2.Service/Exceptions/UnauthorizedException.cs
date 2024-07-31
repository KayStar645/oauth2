using Microsoft.AspNetCore.Http;

namespace OAuth2.Service.Exceptions
{
    public class UnauthorizedException : ApplicationException
    {
        public int ErrorCode { get; } = StatusCodes.Status401Unauthorized;

        public UnauthorizedException(int errorCode) 
            : base("The user is not authorized to access this resource!")
        {
            ErrorCode = errorCode;
        }
    }
}
