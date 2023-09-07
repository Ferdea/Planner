using System.Net;

namespace Client;

public class Result
{
    public HttpStatusCode Status;

    public Result(HttpStatusCode status)
    {
        Status = status;
    }

    public bool IsErrorStatus => Status.IsErrorStatusCode();

    public static Result Build(HttpStatusCode status) => new(status);

    public static Result<T> Success<T>(T value) =>
        new(HttpStatusCode.OK, value);

    public static Result<T> Error<T>(HttpStatusCode status) =>
        new (status, default);

    public static Result<T> Build<T>(HttpStatusCode status, T value) =>
        new(status, value);
}