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
                case TokenType.Directive: lexed.Add((LexType.Directive, token.Item2)); break;
                case TokenType.Key: lexed.Add((LexType.Key, token.Item2)); break;
                case TokenType.ComplexStart: lexed.Add((LexType.ComplexStart, token.Item2)); break;
                case TokenType.ComplexEnd: lexed.Add((LexType.ComplexEnd, token.Item2)); break;

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

        if (TryLexFloat(value, out lexed))
        {
            return lexed;
        }

        return (LexType.Unknown, value);
    }

    private static bool TryLexInteger(string value, out (LexType, string) lexed)
    {
        int pointer = 0;
        lexed = (LexType.Unknown, value);


        // Check if number is negative
        if (value[pointer] == '-')
        {
            pointer++;
            if (value.Length == 1)
            {
                return false;
            }
        }

        // Hex number.
        if (value[pointer] == '0' && char.ToLower(value[pointer + 1]) == 'x')
        {
            pointer += 2;

            while (pointer < value.Length)
            {
                var ch = char.ToLower(value[pointer]);
                if (char.IsDigit(ch) || (ch >= 'a' && ch <= 'f'))
                {
                    pointer++;
                }
                else
                {
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
                    return false;
                }
            }
            lexed = (LexType.Integer, value);
            return true;
        }
    }

    private static bool TryLexFloat(string value, out (LexType, string) lexed)
    {
        int pointer = 0;
        bool decimalFound = false;
        bool numberFound = false;

        lexed = (LexType.Unknown, value);

        // Check if negative
        if (value[pointer] == '-')
        {
            pointer++;
            if (value.Length == 1)
            {
                return false;
            }
        }

        while (pointer < value.Length)
        {
            var ch = value[pointer++];

            if (pointer == value.Length && numberFound)
            {
                if (value[^1] == 'f')
                {
                    lexed = (LexType.Float, value.Substring(0, value.Length - 1));
                    return true;
                }
            }

            if (ch == '.')
            {
                if (decimalFound)
                {
                    return false;
                }
                decimalFound = true;
                continue;
            }
            
            if (char.IsDigit(ch))
            {
                numberFound = true;
                continue;
            }
            return false;
        }
        lexed = (LexType.Float, value);
        return true;
    }
}
