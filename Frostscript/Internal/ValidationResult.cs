using Frostscript.Features;

namespace Frostscript.Internal
{
    internal interface IValidationResult;

    internal sealed record Pass(ITypedNode Expression) : IValidationResult;
    internal sealed record Fail(Token Token, params string[] Errors) : IValidationResult;

    internal static class IValidationResultExtentions
    {
        public static IValidationResult MergeErrors(this IValidationResult result1, IValidationResult result2) => result1 switch
        {
            Pass => result2,
            Fail fail => result2 switch
            {
                Pass pass => fail,
                Fail previouseFail => new Fail(
                    fail.Token,
                    [.. previouseFail.Errors, .. fail.Errors]
                ),
                _ => throw new InvalidOperationException("Unexpected validation result")
            },
            _ => throw new InvalidOperationException("Unexpected validation result")
        };

        public static IValidationResult Map(this IValidationResult result, Func<ITypedNode, ITypedNode> mapFunc) => result switch
        {
            Pass pass => new Pass(mapFunc(pass.Expression)),
            Fail fail => fail,
            _ => throw new InvalidOperationException("Unexpected validation result")
        };

        public static IValidationResult Bind(this IValidationResult result, Func<ITypedNode, IValidationResult> bindFunc) => result switch
        {
            Pass pass => bindFunc(pass.Expression),
            Fail fail => fail,
            _ => throw new InvalidOperationException("Unexpected validation result")
        };
    }
}
