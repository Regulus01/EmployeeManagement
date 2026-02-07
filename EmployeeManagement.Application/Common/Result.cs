namespace EmployeeManagement.Application.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string[] Errors { get; }

        protected Result(bool isSuccess, string[] errors)
        {
            IsSuccess = isSuccess;
            Errors = errors ?? Array.Empty<string>();
        }

        public static Result Success() => new(true, Array.Empty<string>());
        public static Result Failure(string[] errors) => new(false, errors);
        public static Result<T> Success<T>(T value) => new(value, true, Array.Empty<string>());
        public static Result<T> Failure<T>(string[] errors) => new(default, false, errors);
    }

    public class Result<T> : Result
    {
        public T? Value { get; }

        internal Result(T? value, bool isSuccess, string[] errors) : base(isSuccess, errors)
        {
            Value = value;
        }
    }
}