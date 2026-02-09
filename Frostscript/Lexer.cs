using Frostware.Pipe;

namespace Frostscript
{
    internal static class Lexer
    {
        internal static Token[] Lex(string script)
        {

            Token[] Generate(Token[] tokens, char[] script, int line, int character)
            {
                Token[] Add(TokenType type, int tokensRead = 1, dynamic? literal = null) => 
                    Generate(
                        [.. tokens.Append(new Token(type, line, character, literal))],
                        [.. script.Skip(tokensRead)],
                        line, character
                    );

                if (script.Length == 0)
                    return tokens;
                else
                    return script[0] switch
                    {
                        '+' => Add(TokenType.Plus),
                        '-' => Add(TokenType.Minus),
                        '/' => Add(TokenType.ForwardSlash),
                        '*' => Add(TokenType.Star),

                        ' ' => Generate(tokens, [.. script.Skip(1)], line, character + 1),

                        '"' => new string([.. script.Skip(1).TakeWhile(x => x != '"')])
                            .Pipe(@string => Add(TokenType.Literal, @string.Length + 2, @string)),

                        var x when char.IsLetter(x) =>
                            new string([.. script.TakeWhile(x => char.IsLetter(x))])
                            .Pipe(label => Add(TokenType.Label, label.Length, label)),

                        var x when char.IsNumber(x) =>
                            new string([.. script.TakeWhile(x => char.IsNumber(x) || x == '.')])
                            .Pipe(number => Add(TokenType.Literal, number.Length, decimal.Parse(number))),

                        _ => throw new NotImplementedException()

                    };
            }

            return Generate([], [.. script], 0, 0);
        }
    }
}
