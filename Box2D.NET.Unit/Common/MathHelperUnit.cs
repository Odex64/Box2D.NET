using Box2D.NET.Common;
using Box2D.NET.Unit.Helpers;
using NUnit.Framework;

namespace Box2D.NET.Unit.Common;

[TestFixture]
public class MathHelperUnit
{
    [Test]
    [TestCase(3, false)]
    [TestCase(5, false)]
    [TestCase(1, true)]
    [TestCase(2, true)]
    [TestCase(4, true)]
    [TestCase(8, true)]
    public void IsPowerOfTwo(int value, bool expected)
    {
        bool result = MathHelper.IsPowerOfTwo(value);
        Generics.AreEqual(result, expected);
    }

    [Test]
    [TestCase(3, 4)]
    [TestCase(4, 4)]
    [TestCase(5, 8)]
    [TestCase(7, 8)]
    [TestCase(8, 8)]
    [TestCase(9, 16)]
    public void NextPowerOfTwo(int value, int expected)
    {
        int result = MathHelper.NextPowerOfTwo(value);
        Generics.AreEqual(result, expected);
    }
}