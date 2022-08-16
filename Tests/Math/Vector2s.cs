using FxMath;

namespace Tests.Math;

[TestClass]
public class Vector2s
{
    private Vector2i vec2i;
    private Vector2f vec2f;

    [TestMethod]
    public void TestConstructors()
    {
        vec2i = new();
        vec2f = new();

        Assert.IsTrue(vec2i.x == 0 && vec2i.y == 0);
        Assert.IsTrue(vec2f.x == 0 && vec2f.y == 0);

        vec2i = new Vector2i(4);
        vec2f = new Vector2f(5.4f);

        Assert.IsTrue(vec2i.x == 4 && vec2i.y == 4);
        Assert.IsTrue(vec2f.x == 5.4f && vec2f.y == 5.4f);

        vec2i = new Vector2i(3, 8);
        vec2f = new Vector2f(3.9f, 4.2f);

        Assert.IsTrue(vec2i.x == 3 && vec2i.y == 8);
        Assert.IsTrue(vec2f.x == 3.9f && vec2f.y == 4.2f);
    }

    [TestMethod]
    public void TestMagnitude()
    {
        vec2i = new Vector2i(5, 10);
        vec2f = new Vector2f(4.8f, 9.2f);

        Assert.AreEqual(vec2i.Magnitude(), 11.18f, 0.001f);
        Assert.AreEqual(vec2f.Magnitude(), 10.377f, 0.001f);

        vec2i = new Vector2i(0);
        vec2f = new Vector2f(0.0f);

        Assert.IsTrue(vec2i.Magnitude() == 0);
        Assert.IsTrue(vec2f.Magnitude() == 0);

        vec2i = new Vector2i(-5, 10);
        vec2f = new Vector2f(4.8f, -9.2f);

        Assert.AreEqual(vec2i.Magnitude(), 11.18f, 0.001f);
        Assert.AreEqual(vec2f.Magnitude(), 10.377f, 0.001f);

        vec2i = new Vector2i(-5, -10);
        vec2f = new Vector2f(-4.8f, -9.2f);

        Assert.AreEqual(vec2i.Magnitude(), 11.18f, 0.001f);
        Assert.AreEqual(vec2f.Magnitude(), 10.377f, 0.001f);
    }
}
