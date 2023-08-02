namespace RailwayOrientedProgramming;

public static class ResultExtensions
{

    public static Result<TIn> Tap<TIn>(this Result<TIn> result, Action<TIn> action)
    {
        if (result.IsSuccess)
            action(result.Value);

        return result;
    }

    public static async Task<Result<TIn>> TapAsync<TIn>(this Task<Result<TIn>> result, Func<Result<TIn>, Task> func)
    {
        var res = await result;
        if (res.IsSuccess)
            await func(res);

        return res;
    }

    public static Result<TOut> Bind<TIn, TOut>(this Result<TIn> result, Func<TIn, Result<TOut>> func)
    {
        return result.IsFailure ? Result<TOut>.Fail(result.Error) : func(result.Value);
    }

    public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this Result<TIn> result, Func<TIn, Task<Result<TOut>>> func)
    {
        return result.IsFailure ? Result<TOut>.Fail(result.Error) : await func(result.Value);
    }

     public static Result<(T1, T2)> Combine<T1, T2>(Result<T1> result, Result<T2> result2)
     {
        if (result.IsFailure)
            return Result<(T1, T2)>.Fail(result.Error);

        if (result2.IsFailure)
            return Result<(T1, T2)>.Fail(result2.Error);

        return Result<(T1, T2)>.Ok((result.Value, result2.Value));
     }

     public static Result<TIn> Ensure<TIn>(this Result<TIn> result, Func<TIn, bool> predicate, string message)
     {
        if (result.IsFailure)
            return result;

        return predicate(result.Value) ? result : Result<TIn>.Fail(message);
     }

     public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> mappingFunc)
     {
        return result.IsFailure ?
                Result<TOut>.Fail(result.Error) :
                Result<TOut>.Ok(mappingFunc(result.Value));
     }

     public static async Task<Result<TOut>> MapAsync<TIn, TOut>(this Result<TIn> result, Func<TIn, Task<TOut>> mappingFunc )
     {
        return result.IsFailure ?
                Result<TOut>.Fail(result.Error) :
                Result<TOut>.Ok(await mappingFunc(result.Value));
     }

    public static Result<TOut> Fork<TIn, TOut>(this Result<TIn> result, Func<TIn, bool> predicate,
                                                Func<TIn, TOut> truePathFunc, Func<TIn, TOut> falsePathFunc)
    {
        if (result.IsFailure)
            return Result<TOut>.Fail(result.Error);

        try
        {
            Func<TIn, TOut> functionToRun = predicate(result.Value) ? truePathFunc : falsePathFunc;
            return Result<TOut>.Ok(functionToRun(result.Value));
        }
        catch (Exception ex)
        {
            return Result<TOut>.Fail(ex.Message);
        }
    }

    public static async Task<Result<TOut>> ForkAsync<TIn, TOut>(this Task<Result<TIn>> result, Func<TIn, bool> predicate,
                                                Func<TIn, Task<TOut>> truePathFunc, Func<TIn, Task<TOut>> falsePathFunc)
    {
        var res = await result;
        if (res.IsFailure)
            return Result<TOut>.Fail(res.Error);

        try
        {
            Func<TIn, Task<TOut>> functionToRun = predicate(res.Value) ? truePathFunc : falsePathFunc;
            return Result<TOut>.Ok(await functionToRun(res.Value));
        }
        catch (Exception ex)
        {
            return Result<TOut>.Fail(ex.Message);
        }
    }


}
