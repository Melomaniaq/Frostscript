
namespace Frostscript.Internal
{
    internal record NodeContext(ITypedNode Node, Token Token) 
    {
        internal IDataType DataType => Node.DataType;
    };
}
