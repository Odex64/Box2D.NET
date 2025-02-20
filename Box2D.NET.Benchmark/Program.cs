using BenchmarkDotNet.Running;

namespace Box2D.NET.Benchmark;

public static class Program
{
    private static void Main(string[] args) => _ = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
}