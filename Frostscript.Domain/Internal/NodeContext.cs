
namespace Frostscript.Domain.Internal
{
    public record NodeContext(ITypedNode Node, Token Token) 
    {
        public IDataType DataType => Node.DataType;
    };
}
