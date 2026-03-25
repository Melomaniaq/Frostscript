
namespace Frostscript.Types
{
    public record class FSFunction
    {
        internal string Parameter { get; }
        internal INode Body { get; }
        internal Closure Closure { get; }

        internal FSFunction(string parameter, INode body, Closure closure)
        {
            Parameter = parameter;
            Body = body;
            Closure = closure;
        }
    };
}
