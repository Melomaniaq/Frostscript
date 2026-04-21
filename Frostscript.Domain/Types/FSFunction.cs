using Frostscript.Domain.Features;
using Frostscript.Domain.Features.Models;

namespace Frostscript.Domain.Types
{
    public class FSFunction : ICallable
    {
        public string Parameter { get; }
        public IExpression Body { get; }
        public Closure<string, object> Closure { get; }

        public FSFunction(string parameter, IExpression body, IDictionary<string, object> variables)
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
