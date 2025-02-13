using Box2D.NET.Common.Primitives;
using Box2D.NET.Tests.Common;
using NUnit.Framework;

namespace Box2D.NET.Tests;

[TestFixture]
public class Matrix2x2Tests {
    [Test]
    public void Constructor_ShouldInitializeCorrectly() {
        Matrix2x2 m = new( 1, 2, 3, 4 );
        Generics.AreEqual( m.Ex, new Vector2B( 1, 3 ) );
        Generics.AreEqual( m.Ey, new Vector2B( 2, 4 ) );
    }

    [Test]
    public void SetIdentity_ShouldCreateIdentityMatrix() {
        Matrix2x2 m = new();
        m.SetIdentity();
        Generics.AreEqual( m.Ex, new Vector2B( 1, 0 ) );
        Generics.AreEqual( m.Ey, new Vector2B( 0, 1 ) );
    }

    [Test]
    public void GetInverse_ShouldReturnCorrectInverse() {
        Matrix2x2 m = new( 4, 7, 2, 6 );
        Matrix2x2 inverse = m.GetInverse();
        Generics.AreEqualWithin( inverse, new Matrix2x2( 0.6f, -0.7f, -0.2f, 0.4f ) );
    }

    [Test]
    public void Solve_ShouldReturnCorrectSolution() {
        Matrix2x2 m = new( 4, 7, 2, 6 );
        Vector2B b = new( 5, 9 );
        Vector2B result = m.Solve( b );
        Generics.AreEqualWithin( result, new Vector2B( -3.1f, 4.3f ) );
    }

    [Test]
    public void Multiply_ShouldReturnCorrectResult() {
        Matrix2x2 m1 = new( 1, 2, 3, 4 );
        Matrix2x2 m2 = new( 2, 0, 1, 2 );
        Matrix2x2 result = Matrix2x2.Multiply( m1, m2 );
        Generics.AreEqual( result, new Matrix2x2( 4, 4, 10, 8 ) );
    }
}
