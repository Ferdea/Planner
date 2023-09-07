using System.Net;

namespace Client;

public static class HttpStatusCodeExtensions
{
    public static bool IsErrorStatusCode(this HttpStatusCode statusCode) => 
        (int)statusCode >= 400;
}