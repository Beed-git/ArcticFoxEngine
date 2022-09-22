using ArcticFoxEngine.EC;

namespace ArcticFoxEngine.Scripts;

public class ScriptFactory
{
    private readonly Type _type;

    public ScriptFactory(Type type)
    {
        _type = type;
    }

    public BaseScript CreateScript(Entity parent)
    {
        return (BaseScript)Activator.CreateInstance(_type, parent);
    }
}
