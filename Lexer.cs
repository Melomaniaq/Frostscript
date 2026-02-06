using Frostware.Pipe;

namespace Frostscript
{
    internal static class Lexer
    {
        internal static Token[] Lex(string script)
        {
            Token[] Generate(Token[] tokens, string script, int line, int character)
            {

                if (script.Length == 0)
                    return tokens;
                else
                    return script[0] switch
                    {
                        '"' =>
                            new string([.. script.Skip(1).TakeWhile(x => x != '"')])
                            .Pipe(@string => Generate(
                                [.. tokens.Append(new Token(TokenType.Literal, 0, 0, @string))],
                                new string([.. script.Skip(@string.Length + 2)]),
                                line, character + @string.Length + 2
                            )),

                        var x when char.IsLetter(x) =>
                            new string([.. script.TakeWhile(x => char.IsLetter(x))])
                            .Pipe(label => Generate(
                                [.. tokens.Append(new Token(TokenType.Label, 0, 0, label))],
                                new string([.. script.Skip(label.Length)]),
                                line, character + label.Length
                            )),

                        _ => throw new NotImplementedException()

                    };
            }

            return Generate([], script, 0, 0);
        }
    }
}
