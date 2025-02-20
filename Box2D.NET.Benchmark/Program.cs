using BenchmarkDotNet.Running;
using Box2D.NET.Benchmark.Common.Primitives;

namespace Box2D.NET.Benchmark;

#pragma warning disable IDE0022 // Use expression body for method
#pragma warning disable IDE0040 // Add accessibility modifiers
#pragma warning disable IDE0060 // Remove unused parameter
class Program
{
    static void Main(string[] args)
    {
        _ = BenchmarkRunner.Run<Matrix2x2Benchmark>();
    }
}
