using Box2D.NET.Common;
using NUnit.Framework;

namespace Box2D.NET.Unit.Common;

[TestFixture]
public sealed class MathHelperUnit
{
    [Test]
    [TestCase(1, true)]
    [TestCase(2, true)]
    [TestCase(3, false)]
    [TestCase(4, true)]
    [TestCase(5, false)]
    [TestCase(8, true)]
    public void IsPowerOfTwo(int value, bool expected)
    {
        bool result = MathHelper.IsPowerOfTwo(value);
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    [TestCase(3, 4)]
    [TestCase(4, 8)]
    [TestCase(5, 8)]
    [TestCase(7, 8)]
    [TestCase(8, 16)]
    [TestCase(9, 16)]
    public void NextPowerOfTwo(int value, int expected)
    {
        int result = MathHelper.NextPowerOfTwo(value);
        Assert.That(result, Is.EqualTo(expected));
    }
}