
namespace Frostscript.Domain.Internal
{
    public interface IDataType;
    public struct NumberType : IDataType;
    public struct StringType : IDataType;
    public struct BoolType : IDataType;
    public struct VoidType : IDataType;
    public struct UnknownType : IDataType;
    public record FunctionType(IDataType Parameter, IDataType Body) : IDataType;
}
