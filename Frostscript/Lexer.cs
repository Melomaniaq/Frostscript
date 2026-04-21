using Frostscript.Domain;
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
                        '(' => Add(TokenType.ParenthesesOpen),
                        ')' => Add(TokenType.ParenthesesClose),
                        ';' => Add(TokenType.SemiColon),
                        ':' => Add(TokenType.Colon),
                        '+' => Add(TokenType.Plus),
                        '-' => script[1] switch
                        {
                           '>' => Add(TokenType.Arrow, 2),
                           _ => Add(TokenType.Minus)
                        },
                        '/' => Add(TokenType.ForwardSlash),
                        '*' => Add(TokenType.Star),

                        '=' => script[1] switch
                        {
                            '=' => Add(TokenType.DoubleEqual, 2),
                            _ => Add(TokenType.SingleEqual)
                        },

                        '!' => script[1] switch
                        {
                            '=' => Add(TokenType.NotEqual, 2),
                            _ => Add(TokenType.Not)
                        },

                        '>' => script[1] switch
                        {
                            '=' => Add(TokenType.GreaterOrEqual, 2),
                            _ => Add(TokenType.GreaterThan)
                        },

                        '<' => script[1] switch
                        {
                            '=' => Add(TokenType.LessOrEqual, 2),
                            _ => Add(TokenType.LessThan)
                        },

                        '"' => new string([.. script.Skip(1).TakeWhile(x => x != '"')])
                            .Pipe(@string => Add(TokenType.Literal, @string.Length + 2, @string)),

                        var x when char.IsLetter(x) =>
                            new string([.. script.TakeWhile(x => char.IsLetter(x))])
                            .Pipe(label => label switch
                            {
                                "let" => Add(TokenType.Let, label.Length),
                                "and" => Add(TokenType.And, label.Length),
                                "or" => Add(TokenType.Or, label.Length),
                                "var" => Add(TokenType.Var, label.Length),
                                "fun" => Add(TokenType.Fun, label.Length),
                                "true" => Add(TokenType.Literal, label.Length, true),
                                "false" => Add(TokenType.Literal, label.Length, false),
                                "num" => Add(TokenType.Num, label.Length),
                                "bool" => Add(TokenType.Bool, label.Length),
                                "str" => Add(TokenType.Str, label.Length),
                                _ => Add(TokenType.Label, label.Length, label),
                            }),

                        var x when char.IsNumber(x) =>
                            new string([.. script.TakeWhile(x => char.IsNumber(x) || x == '.')])
                            .Pipe(number => Add(TokenType.Literal, number.Length, decimal.Parse(number))),

                        ' ' or '\t' => Generate(tokens, [.. script.Skip(1)], line, character + 1),
                        '\n' => Generate(tokens, [.. script.Skip(1)], line + 1, 0),
                        '\r' => Generate(tokens, [.. script.Skip(1)], line, character),
                        _ => throw new NotImplementedException()
                    };
            }
            return Generate([], [.. script, ' '], 0, 0);
        }
    }
}
