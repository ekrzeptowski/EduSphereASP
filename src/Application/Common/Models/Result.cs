namespace EduSphere.Application.Common.Models;

public class Result
{
    internal Result(bool succeeded, IEnumerable<string> errors, string data = "")
    {
        Succeeded = succeeded;
        Errors = errors.ToArray();
        Data = data;
    }

    public bool Succeeded { get; init; }

    public string[] Errors { get; init; }

    public string Data { get; init; }

    public static Result Success()
    {
        return new Result(true, Array.Empty<string>());
    }

    public static Result Success(string data)
    {
        return new Result(true, Array.Empty<string>(), data);
    }

    public static Result Failure(IEnumerable<string> errors)
    {
        return new Result(false, errors);
    }
}
