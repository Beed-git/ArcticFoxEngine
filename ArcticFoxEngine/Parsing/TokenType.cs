namespace ArcticFoxEngine.Parsing;

public record struct Token(TokenType Type, string Value);

public enum TokenType
{
    Directive,
    Key,
    ComplexStart,
    ComplexEnd,
    Value,
}
