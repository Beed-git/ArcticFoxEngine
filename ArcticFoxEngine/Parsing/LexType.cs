namespace ArcticFoxEngine.Parsing;

public record struct Lex(LexType Type, string Value);

public enum LexType
{
    Unknown,
    Key,
    ComplexStart,
    ComplexEnd,

    String,
    Integer,
    Float,
}
