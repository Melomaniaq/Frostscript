
namespace Frostscript.Internal
{
    internal interface IDataType;
    internal struct NumberType : IDataType;
    internal struct StringType : IDataType;
    internal struct BoolType : IDataType;
    internal struct VoidType : IDataType;
    internal struct UnknownType : IDataType;
    internal record FunctionType(IDataType Parameter, IDataType Body) : IDataType;
}
