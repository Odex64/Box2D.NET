using Box2D.NET.Common;
using NUnit.Framework;

namespace Box2D.NET.Unit.Common;

[TestFixture]
public sealed class UtilsUnit
{
    [Test]
    public void SwapValues()
    {
        int a = 10, b = 20;
        Utils.Swap(ref a, ref b);

        Assert.Multiple(() =>
        {
            Assert.That(a, Is.EqualTo(20));
            Assert.That(b, Is.EqualTo(10));
        });
    }

    [Test]
    public void SwapObjects()
    {
        string a = "hello", b = "world";
        Utils.Swap(ref a, ref b);

        Assert.Multiple(() =>
        {
            Assert.That(a, Is.EqualTo("world"));
            Assert.That(b, Is.EqualTo("hello"));
        });
    }
}