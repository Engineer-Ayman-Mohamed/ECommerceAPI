namespace ECommerceAPI.Application.Shared;

public class Result
{
    public bool IsSuccess { get; }
    public string? Error { get; }
    public string? ErrorCode { get; }

    private Result(bool isSuccess, string? error, string? errorCode)
    {
        IsSuccess = isSuccess;
        Error = error;
        ErrorCode = errorCode;
    }

    public static Result Ok() => new(true, null, null);
    public static Result Fail(string error, string? errorCode = null) => new(false, error, errorCode);
}

public class Result<T>
{
    public T? Value { get; }
    public string? Error { get; }
    public string? ErrorCode { get; }
    public bool IsSuccess { get; }

    private Result(T? value, string? error, string? errorCode, bool isSuccess)
    {
        Value = value;
        Error = error;
        ErrorCode = errorCode;
        IsSuccess = isSuccess;
    }

    public static Result<T> Ok(T value) => new(value, null, null, true);
    public static Result<T> Fail(string error, string? errorCode = null) => new(default, error, errorCode, false);
}
