
using Frostscript.Features;
using Frostscript.Internal;

namespace Frostscript.Types
{
    public class FSFunction : ICallable
    {
        internal string Parameter { get; }
        internal IExpression Body { get; }
        internal IDictionary<string, object> Closure { get; }

        internal FSFunction(string parameter, IExpression body, IDictionary<string, object> closure)
        {
            Parameter = parameter;
            Body = body;
            Closure = closure;
        }

        public dynamic Call(dynamic value)
        {
            Closure[Parameter] = value;
            return ExpressionTree.ExpressionTree.Interpret(Body, Closure);
        }

        public T Call<T>(dynamic value) => (T)Call(value);
    };
}
