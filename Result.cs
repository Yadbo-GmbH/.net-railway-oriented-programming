namespace RailwayOrientedProgramming;

public class Result<T> : IResult
{
    public bool Success { get; set; }

    public string Error { get; set; }

    public bool IsFailure => !Success;

    public bool IsSuccess => Success;

    public T? Value { get; set; }

    protected internal Result(bool success, string error) {
        if (success && error != string.Empty)
            throw new InvalidOperationException();

        if (!success && error == string.Empty)
            throw new InvalidOperationException();

        Success = success;
        Error = error;
    }

    protected internal Result(T value, bool success, string error) : this(success, error)
    {
        Value = value;
    }

    public static Result<T> Create(T value)
    {
        return new Result<T>(value, true, string.Empty);
    }

    public static Result<T> Fail(string message)
    {
        return new Result<T>(default, false, message);
    }

    public static Result<T> Ok()
    {
        return new Result<T>(default, true, string.Empty);
    }

    public static Result<T> Ok(T value)
    {
        return new Result<T>(value, true, string.Empty);
    }

}
