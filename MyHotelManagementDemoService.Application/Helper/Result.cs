using System.Net;

namespace BlogApp.Application.Helpers
{
    public class Result<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
        public HttpStatusCode statusCode { get; set; }

        public static Result<T> SuccessResult(T data)
        {
            return new Result<T>
            {
                Success = true,
                Data = data,
                statusCode = HttpStatusCode.Created

            };
        }

        public static Result<T> SuccessResult(string message)
        {
            return new Result<T>
            {
                Success = true,
                Message = message,
                statusCode = HttpStatusCode.OK

            };
        }


        public static Result<T> InternalServerError()
        {
            return new Result<T>
            {
                Success = false,
                Message = "Something Went Wrong",
                statusCode = HttpStatusCode.InternalServerError
            };
        }
        public static Result<T> BadRequest()
        {
            return new Result<T>
            {
                Success = false,
                Message = " Invalid Credentials",
                statusCode = HttpStatusCode.BadRequest
            };
        }

        public static Result<T> NotFound(string errorMessage)
        {
            return new Result<T>
            {
                Success = false,
                Message = errorMessage,
                statusCode = HttpStatusCode.NotFound
            };
        }

        public static Result<T> Conflict(string errorMessage)
        {
            return new Result<T>
            {
                Success = false,
                Message = errorMessage,
                statusCode = HttpStatusCode.Conflict
            };
        }
    }
}