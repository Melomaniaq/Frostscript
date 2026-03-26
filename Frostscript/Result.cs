namespace Frostscript
{
    internal interface IResult;
    internal record struct Pass() : IResult;
    internal record struct Fail(string Error) : IResult;
}

