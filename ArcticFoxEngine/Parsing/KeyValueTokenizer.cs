using System.Text;

namespace ArcticFoxEngine.Parsing;

public class KeyValueTokenizer
{
    private string _text;
    private int _pointer;

    private readonly List<Token> _tokens;

    private bool CanRead => _pointer < _text.Length;

    public KeyValueTokenizer()
    {
        _text = string.Empty;
        _pointer = 0;

        _tokens = new();
    }

    public IEnumerable<Token> Parse(string text)
    {
        _text = text;
        _pointer = 0;

        _tokens.Clear();

        while (CanRead)
        {
            ConsumeWhitespace();

            if (CanRead)
            {
                if (PeekChar() == '@')
                {
                    ConsumeChar();
                    var directive = ConsumeWord(true);
                    _tokens.Add(new Token(TokenType.Directive, directive));
                    continue;
                }

                ConsumeKeyValue();
            }
        }

        return _tokens;
    }

    private void ConsumeKeyValue()
    {
        var key = ConsumeWord(true);
        _tokens.Add(new Token(TokenType.Key, key));

        ConsumeWhitespace();

        if (ConsumeChar() != '=')
        {
            throw new Exception("Expected = sign.");
        }

        ConsumeWhitespace();

        if (CanRead)
        {
            if (PeekChar() == '{')
            {
                ConsumeComplex();
                return;
            }

            if (PeekChar() == '(')
            {
                ConsumeArray();
                return;
            }

            var value = ConsumeStringOrWord();
            _tokens.Add(new Token(TokenType.Value, value));
        }
        else throw new Exception($"No value exists after '{key}='");
    }

    private void ConsumeComplex()
    {
        ConsumeWhitespace();

        var start = ConsumeChar();
        if (start != '{')
        {
            throw new Exception("Failed to find complex beginning!");
        }
        _tokens.Add(new Token(TokenType.ComplexStart, string.Empty));

        while (CanRead)
        {
            ConsumeWhitespace();

            if (!CanRead)
            {
                throw new Exception("Failed to find complex end!");
            }
            if (PeekChar() == '}')
            {
                break;
            }

            ConsumeKeyValue();
        }

        var end = ConsumeChar();
        if (end != '}')
        {
            throw new Exception("Failed to find complex end!");
        }
        _tokens.Add(new Token(TokenType.ComplexEnd, string.Empty));
    }

    private void ConsumeArray()
    {
        ConsumeWhitespace();

        var start = ConsumeChar();
        if (start is not '(' or '[')
        {
            throw new Exception("Failed to find array beginning!");
        }

        while (CanRead)
        {
            ConsumeWhitespace();

            if (!CanRead)
            {
                throw new Exception("Failed to find array end!");
            }

            if (PeekChar() is ')' or ']')
            {
                break;
            }

            if (PeekChar() is ',')
            {
                ConsumeChar();
            }

            var value = ConsumeStringOrWord();
            _tokens.Add(new Token(TokenType.Value, value));
        }

        var end = ConsumeChar();
        if (start == '(' && end == '[')
        {
            throw new Exception("Array which starts with '(' cannot end with ']'");
        }
        else if (start == '[' && end == '(')
        {
            throw new Exception("Array which starts with '[' cannot end with ')");
        }
    }

    private string ConsumeStringOrWord()
    {
        if (PeekChar() == '\"')
        {
            return ConsumeString();
        }
        else
        {
            return ConsumeWord(false);
        }
    }

    private string ConsumeString()
    {
        ConsumeWhitespace();

        var builder = new StringBuilder();
        bool end = false;

        var start = ConsumeChar();
        if (start != '\"')
        {
            throw new Exception("Failed to find string!");
        }
        builder.Append(start);

        while (CanRead)
        {
            var ch = ConsumeChar();

            if (ch == '\\')
            {
                ch = ConsumeChar();
                builder.Append(ch);
                continue;
            }

            builder.Append(ch);

            if (ch == '\"')
            {
                end = true;
                break;
            }
        }

        if (!end)
        {
            throw new Exception("Failed to find end of string character!");
        }

        return builder.ToString();
    }

    private string ConsumeWord(bool toLower)
    {
        ConsumeWhitespace();

        var builder = new StringBuilder();
        while (CanRead)
        {
            var peek = PeekChar();

            // If we've reached whitespace, an equals, or an end character we're finished.
            if (char.IsWhiteSpace(peek) || peek == '=' || IsEndCharacter(peek))
            {
                break;
            }

            var ch = ConsumeChar();
            if (toLower)
            {
                ch = char.ToLower(ch);
            }
            if (IsValidCharacter(ch))
            {
                builder.Append(ch);
            }
            else throw new Exception($"Invalid character '{ch}' while parsing word '{builder}'");
        }
        return builder.ToString();
    }

    private char PeekChar()
    {
        return _text[_pointer];
    }

    private char ConsumeChar()
    {
        return _text[_pointer++];
    }

    private void ConsumeWhitespace()
    {
        while (CanRead)
        {
            if (char.IsWhiteSpace(PeekChar()))
            {
                _pointer++;
            }
            else return;
        }
    }

    private static bool IsValidCharacter(char ch)
    {
        return char.IsLetterOrDigit(ch) || ch == '-' || ch == '_' || ch == '.';
    }

    private static bool IsEndCharacter(char ch)
    {
        return ch == '}' || ch == ']' || ch == ')' || ch == ',';
    }
}
