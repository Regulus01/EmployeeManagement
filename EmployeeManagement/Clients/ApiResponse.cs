public class ApiResponse
{
    public bool Success { get; }
    public List<string> Errors { get; }

    private ApiResponse(bool success, List<string>? errors = null)
    {
        Success = success;
        Errors = errors ?? new();
    }

    public static ApiResponse Ok() => new(true);
    public static ApiResponse Fail(List<string> errors) => new(false, errors);
}