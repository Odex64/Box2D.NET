using NUnit.Framework.Constraints;

namespace Box2D.NET.Unit.Helpers;

public static class Generics
{
    public static EqualNumericWithoutUsingConstraint<float> ToleranceEqualTo(float expected) => new EqualNumericConstraint<float>(expected).Within(1e-6f);
    public static EqualNumericWithoutUsingConstraint<double> ToleranceEqualTo(double expected) => new EqualNumericConstraint<double>(expected).Within(1e-6);
}