namespace Frostscript.Domain
{
    public interface IResult<TPass, TFail>
    {
        public sealed record Pass(TPass Value) : IResult<TPass, TFail>;
        public sealed record Fail(TFail Value) : IResult<TPass, TFail>;
    }

    public static class IResultExtentions
    {
        public static IResult<TResult, TFail> Map<TPass, TFail, TResult>(this IResult<TPass, TFail> result, Func<TPass, TResult> mapFunc) => result switch
        {
            IResult<TPass, TFail>.Pass pass => new IResult<TResult, TFail>.Pass(mapFunc(pass.Value)),
            IResult<TPass, TFail>.Fail fail => new IResult<TResult, TFail>.Fail(fail.Value),
            _ => throw new InvalidOperationException("Unexpected validation result")
        };

        public static IResult<TPass, TResult> MapFailure<TPass, TFail, TResult>(this IResult<TPass, TFail> result, Func<TFail, TResult> mapFunc) => result switch
        {
            IResult<TPass, TFail>.Pass pass => new IResult<TPass, TResult>.Pass(pass.Value),
            IResult<TPass, TFail>.Fail fail => new IResult<TPass, TResult>.Fail(mapFunc(fail.Value)),
            _ => throw new InvalidOperationException("Unexpected validation result")
        };

        public static IResult<TPass, TResult> BindFailure<TPass, TFail, TResult>(this IResult<TPass, TFail> result, Func<TFail, IResult<TPass, TResult>> bindFunc) => result switch
        {
            IResult<TPass, TFail>.Pass pass => new IResult<TPass, TResult>.Pass(pass.Value),
            IResult<TPass, TFail>.Fail fail => bindFunc(fail.Value),
            _ => throw new InvalidOperationException("Unexpected validation result")
        };

        public static IResult<TResult, TFail> Bind<TPass, TFail, TResult>(this IResult<TPass, TFail> result, Func<TPass, IResult<TResult, TFail>> bindFunc) => result switch
        {
            IResult<TPass, TFail>.Pass pass => bindFunc(pass.Value),
            IResult<TPass, TFail>.Fail fail => new IResult<TResult, TFail>.Fail(fail.Value),
            _ => throw new InvalidOperationException("Unexpected result")
        };
    }
}
