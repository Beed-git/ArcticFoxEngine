namespace ArcticFoxEngine.Services;

public enum Requirement
{
    Required,
    Optional,
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class InjectAttribute : Attribute
{
    public InjectAttribute() : this(Requirement.Optional)
    {
    }

    public InjectAttribute(Requirement requirement)
    {
        Requirement = requirement;
    }

    public Requirement Requirement { get; private init; }
}
