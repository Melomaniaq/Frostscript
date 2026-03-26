
namespace Frostscript.Internal
{
    internal record NodeContext(INode Node, Token Token) 
    {
        internal IDataType DataType => Node.DataType;
    };
}
