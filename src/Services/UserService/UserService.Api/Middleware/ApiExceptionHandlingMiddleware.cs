using UserService.Application.Exceptions;

namespace UserService.Api.Middleware
{
    public sealed class ApiExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (UserAlreadyExistsException ex)
            {
                await WriteProblemAsync(context, StatusCodes.Status409Conflict, "User already exists", ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                await WriteProblemAsync(context, StatusCodes.Status401Unauthorized, "Unauthorized", ex.Message);
            }
            catch (Exception ex)
            {
                await WriteProblemAsync(context, StatusCodes.Status500InternalServerError, "Internal server error", ex.Message);
            }
        }

        private static Task WriteProblemAsync(HttpContext context, int statusCode, string title, string detail)
        {
            context.Response.Clear();
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/problem+json";

            return Results.Problem(title: title, detail: detail, statusCode: statusCode).ExecuteAsync(context);
        }
    }
}