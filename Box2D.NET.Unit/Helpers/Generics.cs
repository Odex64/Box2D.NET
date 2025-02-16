using System;
using Box2D.NET.Common.Primitives;
using NUnit.Framework.Constraints;

namespace Box2D.NET.Unit.Helpers;

public static class Generics
{
    private const float _tolerance = 1e-6f;
    public static EqualNumericWithoutUsingConstraint<float> ToleranceEqualTo(float expected) => new EqualNumericConstraint<float>(expected).Within(_tolerance);
    public static EqualNumericWithoutUsingConstraint<double> ToleranceEqualTo(double expected) => new EqualNumericConstraint<double>(expected).Within(_tolerance);

}