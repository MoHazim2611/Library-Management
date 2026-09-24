namespace LibraryApi.Common;

public enum ErrorType { None, NotFound, Validation, Conflict }

/// نتيجة أي عملية في الـ Service: نجحت ولا فشلت، ولو فشلت ليه.
public class ServiceResult<T>
{
    public bool Success { get; private set; }
    public T? Data { get; private set; }
    public string? Error { get; private set; }
    public ErrorType ErrorType { get; private set; }

    public static ServiceResult<T> Ok(T data) => new() { Success = true, Data = data };
    public static ServiceResult<T> NotFound(string msg) => new() { Error = msg, ErrorType = ErrorType.NotFound };
    public static ServiceResult<T> Invalid(string msg) => new() { Error = msg, ErrorType = ErrorType.Validation };
    public static ServiceResult<T> Conflict(string msg) => new() { Error = msg, ErrorType = ErrorType.Conflict };
}
