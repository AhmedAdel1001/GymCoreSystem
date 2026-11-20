namespace GymManagementSystem.BLL.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public string ErrorMessage { get; }

        protected Result(bool isSuccess, string errorMessage)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        public static Result Ok() => new Result(true, string.Empty);
        public static Result Fail(string error) => new Result(false, error);
    }

    public class Result<T> : Result
    {
        public T? Value { get; }

        protected Result(bool isSuccess, T? value, string errorMessage) : base(isSuccess, errorMessage)
        {
            Value = value;
        }

        public static Result<T> Ok(T value) => new Result<T>(true, value, string.Empty);
        public static new Result<T> Fail(string error) => new Result<T>(false, default, error);
    }
}

