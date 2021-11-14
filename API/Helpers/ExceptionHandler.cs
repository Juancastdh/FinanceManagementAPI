using FinanceManagement.Core.Exceptions;
using System.Net;

namespace FinanceManagement.API.Helpers
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate next;

        public ExceptionHandler(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {

            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                await HandleException(context, exception);
            }
        }

        private static Task HandleException(HttpContext context, Exception exception)
        {
            HttpStatusCode code;
            string result = "";

            if (exception is DataNotFoundException)
            {
                code = HttpStatusCode.NotFound;
            }
            else
            {
                code = HttpStatusCode.InternalServerError;
            }

            context.Response.StatusCode = (int)code;
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsync(result);

        }

    }
}
