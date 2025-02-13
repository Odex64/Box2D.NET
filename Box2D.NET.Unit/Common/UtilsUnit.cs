using Box2D.NET.Common;
using Box2D.NET.Unit.Helpers;
using NUnit.Framework;

namespace Box2D.NET.Unit.Common;

[TestFixture]
public class UtilsUnit
{
    [Test]
    public void SwapValues()
    {
        int a = 5, b = 10;
        Utils.Swap(ref a, ref b);
        Generics.AreEqual(b, 5);
    }

    [Test]
    public void SwapObjects()
    {
        string str1 = "hello", str2 = "world";
        Utils.Swap(ref str1, ref str2);
        Generics.AreEqual(str2, "hello");
        Generics.AreEqual(str1, "world");
    }
}