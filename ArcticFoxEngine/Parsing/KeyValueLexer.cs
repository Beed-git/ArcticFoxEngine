namespace ArcticFoxEngine.Parsing;

public class KeyValueLexer
{
    public KeyValueLexer()
    {
        StrictArrays = true;
    }

    public Dictionary<string, List<Lex>> Lex(IEnumerable<Token> tokens)
    {
        var lexed = new Dictionary<string, List<Lex>>();

        TokenType lastToken = TokenType.Key;
        LexType lastLex = LexType.Unknown;

        List<Lex>? currentDirective = null;
        Lex lex;
        foreach (var (type, value) in tokens)
        {
            if (type == TokenType.Directive)
            {
                if (!lexed.TryGetValue(value, out currentDirective))
                {
                    currentDirective = new List<Lex>();
                    lexed.Add(value, currentDirective);
                }
                continue;
            }
            else if (type == TokenType.Value)
            {
                lex = LexValueToken(value);
                if (StrictArrays && 
                    lastToken == TokenType.Value &&
                    lastLex != lex.Type)
                {
                    throw new Exception($"Array mixed types {lastLex} and {lex.Type}");
                }
            }
            else
            {
                lex = type switch
                {
                    TokenType.Key => new Lex(LexType.Key, value),
                    TokenType.ComplexStart => new Lex(LexType.ComplexStart, value),
                    TokenType.ComplexEnd => new Lex(LexType.ComplexEnd, value),
                    _ => new Lex(LexType.Unknown, value)
                };
            }

            lastToken = type;
            lastLex = lex.Type;
            if (currentDirective is null)
            {
                throw new Exception("Directive not set!");
            }
            currentDirective.Add(lex);
        }

        return lexed;
    }

    public bool StrictArrays { get; set; }

    private static Lex LexValueToken(string value)
    {
        if (value.StartsWith('"'))
        {
            if (value.EndsWith('"'))
            {
                return new Lex(LexType.String, value);
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

        return new Lex(LexType.Unknown, value);
    }

    private static bool TryLexInteger(string value, out Lex lexed)
    {
        int pointer = 0;
        lexed = new Lex(LexType.Unknown, value);

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
        if (value.Length > 1 && value[pointer] == '0' && char.ToLower(value[pointer + 1]) == 'x')
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
            lexed = new Lex(LexType.Integer, number);
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
            lexed = new Lex(LexType.Integer, value);
            return true;
        }
    }

    private static bool TryLexFloat(string value, out Lex lexed)
    {
        int pointer = 0;
        bool decimalFound = false;
        bool numberFound = false;

        lexed = new Lex(LexType.Unknown, value);

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
                    lexed = new Lex(LexType.Float, value[..^1]);
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
        lexed = new Lex(LexType.Float, value);
        return true;
    }
}
