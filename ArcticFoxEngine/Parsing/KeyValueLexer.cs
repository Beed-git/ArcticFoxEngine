namespace ArcticFoxEngine.Parsing;

public class KeyValueLexer
{
    public IEnumerable<(LexType, string)> Lex(IEnumerable<(TokenType, string)> tokens)
    {
        var lexed = new List<(LexType, string)>();

        foreach (var token in tokens)
        {
            switch (token.Item1)
            {
                case TokenType.Directive: lexed.Add((LexType.Directive, string.Empty)); break;
                case TokenType.Key: lexed.Add((LexType.Key, string.Empty)); break;
                case TokenType.ComplexStart: lexed.Add((LexType.ComplexStart, string.Empty)); break;
                case TokenType.ComplexEnd: lexed.Add((LexType.ComplexEnd, string.Empty)); break;

                case TokenType.Value: lexed.Add(LexValueToken(token.Item2)); break;
            }
        }

        return lexed;
    }

    private static (LexType, string) LexValueToken(string value)
    {
        if (value.StartsWith('"'))
        {
            if (value.EndsWith('"'))
            {
                return (LexType.String, value);
            }
            else throw new Exception($"Invalid string, string does not end with '\"'\n{value}");
        }

        if (TryLexInteger(value, out var lexed))
        {
            return lexed;
        }

        return (LexType.Unknown, value);
    }

    private static bool TryLexInteger(string value, out (LexType, string) lexed)
    {
        int pointer = 0;

        // Hex number.
        if (value[0] == '0' && char.ToLower(value[1]) == 'x')
        {
            pointer = 2;

            while (pointer < value.Length)
            {
                var ch = char.ToLower(value[pointer]);
                if (char.IsDigit(ch) || (ch >= 'a' && ch <= 'f'))
                {
                    pointer++;
                }
                else
                {
                    lexed = (LexType.Unknown, value);
                    return false;
                }
            }
            var number = Convert.ToInt32(value, 16).ToString();
            lexed = (LexType.Integer, number);
            return true;
        }
        else
        {
            // Regular number.
            while (pointer < value.Length)
            {
                if (!char.IsDigit(value[pointer++]))
                {
                    lexed = (LexType.Unknown, value);
                    return false;
                }
            }
            lexed = (LexType.Integer, value);
            return true;
        }
    }
}
