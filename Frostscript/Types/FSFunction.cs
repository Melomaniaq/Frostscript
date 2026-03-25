
using Frostscript.Expressions;

namespace Frostscript.Types
{
    public record class FSFunction : ICallable
    {
        internal string Parameter { get; }
        internal INode Body { get; }
        internal IDictionary<string, object> Closure { get; }

        internal FSFunction(string parameter, INode body, IDictionary<string, object> closure)
        {
            Parameter = parameter;
            Body = body;
            Closure = closure;
        }

        public dynamic Call(dynamic value)
        {
            Closure[Parameter] = value;
            return Expression.ExpressionTree.Interpret(Body, Closure);
        }

        public T Call<T>(dynamic value) => (T)Call(value);
    };
}
