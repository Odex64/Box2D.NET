using Box2D.NET.Common.Primitives;
using Box2D.NET.Tests.Common;
using NUnit.Framework;

namespace Box2D.NET.Tests;

[TestFixture]
public class Vector2BTests {
    [Test]
    public void Constructor_ShouldInitializeCorrectly() {
        Vector2B vector = new( 3, 4 );
        Generics.AreEqual( vector.X, 3 );
        Generics.AreEqual( vector.Y, 4 );
    }

    [Test]
    public void Length_ShouldReturnCorrectValue() {
        Vector2B vector = new( 3, 4 );
        Generics.AreEqual( vector.Length(), 5 );
    }

    [Test]
    public void Normalize_ShouldReturnUnitVector() {
        Vector2B vector = new( 3, 4 );
        float length = vector.Normalize();
        Generics.AreEqual( vector.Length(), 1 );
        Generics.AreEqual( length, 5 );
    }

    [Test]
    public void AddOperator_ShouldReturnCorrectVector() {
        Vector2B v1 = new( 1, 2 );
        Vector2B v2 = new( 3, 4 );
        Vector2B result = v1 + v2;
        Generics.AreEqual( result, new Vector2B( 4, 6 ) );
    }

    [Test]
    public void SubtractOperator_ShouldReturnCorrectVector() {
        Vector2B v1 = new( 5, 7 );
        Vector2B v2 = new( 2, 3 );
        Vector2B result = v1 - v2;
        Generics.AreEqual( result, new Vector2B( 3, 4 ) );
    }

    [Test]
    public void Dot_ShouldReturnCorrectValue() {
        Vector2B v1 = new( 2, 3 );
        Vector2B v2 = new( 4, 5 );
        float result = Vector2B.Dot( v1, v2 );
        Generics.AreEqual( result, 23 );
    }

    [Test]
    public void Cross_ShouldReturnCorrectValue() {
        var v1 = new Vector2B( 2, 3 );
        var v2 = new Vector2B( 4, 5 );
        float result = Vector2B.Cross( v1, v2 );
        Generics.AreEqual( result, -2 );
    }
}
