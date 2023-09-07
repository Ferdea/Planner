using System.Net;

namespace Client;

public class Result<T>
{
    public HttpStatusCode Status => result.Status;

    public bool IsErrorStatus => result.IsErrorStatus;
    
    public T? Value;

    public Result(HttpStatusCode status, T? value)
    {
        result = new Result(status);
        Value = value;
    }
    
    internal Result(Result result, T? value)
    {
        this.result = result;
        Value = value;
    }
    
    private Result result;
}