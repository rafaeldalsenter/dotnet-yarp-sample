using System.Net;

namespace Dotnet.YarpSample.Extensions;

public static class HttpContextExtensions
{
    public static void SetInternalServerError(this HttpContext context) => context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
}