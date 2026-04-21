namespace Frostscript.Domain
{
    public static class IEnumerableExtentions
    {
        public static IResult<TPass[], TFail[]> Traverse<T, TPass, TFail>(this IEnumerable<T> enumerable, Func<T, IResult<TPass, TFail>> resultFunc)
        {
            return enumerable.Aggregate(new IResult<TPass[], TFail[]>.Pass([]) as IResult<TPass[], TFail[]>, (current, next) => (current, resultFunc(next)) switch
            {
                (IResult<TPass[], TFail[]>.Pass pass, IResult<TPass, TFail>.Pass newPass) => 
                    new IResult<TPass[], TFail[]>.Pass([.. pass.Value.Append(newPass.Value)]),

                (IResult<TPass[], TFail[]>.Fail fail, IResult<TPass, TFail>.Pass) => 
                    fail,

                (IResult<TPass[], TFail[]>.Pass pass, IResult<TPass, TFail>.Fail newFail) => 
                    new IResult<TPass[], TFail[]>.Fail([newFail.Value]),

                (IResult<TPass[], TFail[]>.Fail fail, IResult<TPass, TFail>.Fail newFail) =>
                    new IResult<TPass[], TFail[]>.Fail([.. fail.Value, newFail.Value]),
            });
        }

        public static IResult<TPass[], TFail[]> Traverse<TPass, TFail>(this IEnumerable<IResult<TPass, TFail>> enumerable) => Traverse(enumerable, x => x);
    }
}
