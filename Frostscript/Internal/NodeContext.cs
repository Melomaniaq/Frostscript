
namespace Frostscript.Internal
{
    internal record NodeContext(IExpression Node, Token Token) 
    {
        internal IDataType DataType => Node.DataType;
    };
}
