namespace RailwayOrientedProgramming;

public interface IResult
{
    public bool Success { get; }
    public string Error { get; }
    public bool IsFailure => !Success;
    public bool IsSuccess => !Success;
}


