using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;
using Box2D.NET.Benchmark.Helpers;
using Box2D.NET.Common.Primitives;

namespace Box2D.NET.Benchmark.Common.Primitives;

[MemoryDiagnoser]
[Config(typeof(Config))]
[BenchmarkCategory(Categories.Primitive)]
[SimpleJob(1, 1, 10000)]
public class Matrix2x2Benchmark
{
    private readonly Matrix2x2 _matrix = new Matrix2x2(1, 2, 3, 4);
    private readonly Vector2 _vector = new Vector2(5, 6);

    [Benchmark(OperationsPerInvoke = 10_000)]
    public Matrix2x2 MultiplyMatrix() => Matrix2x2.Multiply(_matrix, _matrix);

    [Benchmark(OperationsPerInvoke = 10_000)]
    public Vector2 MultiplyVector() => Matrix2x2.Multiply(_matrix, _vector);

    [Benchmark(OperationsPerInvoke = 10_000)]
    public Matrix2x2 GetInverse() => _matrix.GetInverse();

    private class Config : ManualConfig
    {
        public Config()
        {
            _ = AddColumn(StatisticColumn.AllStatistics);
            _ = AddDiagnoser(MemoryDiagnoser.Default);
            _ = AddLogger(ConsoleLogger.Default);
            SummaryStyle = SummaryStyle.Default.WithRatioStyle(RatioStyle.Trend);
        }
    }
}