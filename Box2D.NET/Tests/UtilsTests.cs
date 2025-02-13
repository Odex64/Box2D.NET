using Box2D.NET.Common;
using Box2D.NET.Tests.Common;
using NUnit.Framework;

namespace Box2D.NET.Tests;

[TestFixture]
public class UtilsTests {
    [Test]
    public void Swap_ShouldSwapValues() {
        int a = 5, b = 10;
        Utils.Swap( ref a, ref b );
        Generics.AreEqual( b, 5 );
    }

    [Test]
    public void Swap_ShouldSwapObjects() {
        string str1 = "hello", str2 = "world";
        Utils.Swap( ref str1, ref str2 );
        Generics.AreEqual( str2, "hello" );
        Generics.AreEqual( str1, "world" );
    }
}
