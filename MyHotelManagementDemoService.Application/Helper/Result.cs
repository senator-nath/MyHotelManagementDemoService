using System.Net;

namespace BlogApp.Application.Helpers
{
    public class Result<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
        public HttpStatusCode statusCode { get; set; }

        public static Result<T> SuccessResult(T data, HttpStatusCode statusCode)
        {
            return new Result<T>
            {
                Success = true,
                Data = data,
                statusCode = statusCode

            };
        }

         public static Result<T> SuccessResult(string message, HttpStatusCode statusCode)
        {
            return new Result<T>
            {
                Success = true,
                Message = message,
                statusCode = statusCode

            };
        }


        public static Result<T> ErrorResult(string errorMessage, HttpStatusCode statusCode)
        {
            return new Result<T>
            {
                Success = false,
                Message = errorMessage,
                statusCode = statusCode
            };
        } 
    }
}