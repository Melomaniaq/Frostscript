
using Frostscript.Features;
using Frostscript.Internal;

namespace Frostscript.Types
{
    public class FSFunction : ICallable
    {
        internal string Parameter { get; }
        internal IExpression Body { get; }
        internal Closure<string, object> Closure { get; }

        internal FSFunction(string parameter, IExpression body, IDictionary<string, object> variables)
        {
            Parameter = parameter;
            Body = body;
            Closure = new Closure<string, object>(variables);
        }

        public dynamic Call(dynamic value)
        {
            Closure[Parameter] = value;
            return ExpressionTree.Interpret(Body, Closure);
        }

        public T Call<T>(dynamic value) => (T)Call(value);
    };
}
