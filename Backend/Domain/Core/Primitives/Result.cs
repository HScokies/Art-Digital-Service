namespace Domain.Core.Primitives
{
    public readonly struct Result<T>
    {
        public readonly T value;
        public readonly Error error;
        public readonly bool isSuccess;

        public Result(T value)
        {
            this.value = value;
            this.error = null!;
            isSuccess = true;
        }
        public Result(Error error)
        {
            this.value = default(T)!;
            this.error = error;
            isSuccess = false;
        }

        public R Match<R>(Func<T, R> Success, Func<Error, R> Fail)
        {
            return isSuccess ? Success(value) : Fail(error);
        }
    }
}
