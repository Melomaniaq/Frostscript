
namespace Frostscript.Internal
{
    internal interface IDataType;
    internal record struct NumberType : IDataType;
    internal record struct StringType : IDataType;
    internal record struct BoolType : IDataType;
    internal record struct VoidType : IDataType;
    internal record struct UnknownType : IDataType;
    internal record FunctionType(IDataType Parameter, IDataType Body) : IDataType;
}
