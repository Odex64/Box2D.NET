using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Reports;
using Box2D.NET.Common.Primitives;

namespace Box2D.NET.Benchmark.Common.Primitives;

[MemoryDiagnoser]
[HardwareCounters(HardwareCounter.BranchMispredictions, HardwareCounter.CacheMisses)]
[Config(typeof(Config))]
[SimpleJob(RuntimeMoniker.Net90, launchCount: 1, warmupCount: 1, iterationCount: 10000)]
public class Matrix2x2Benchmark
{
    private Matrix2x2 _matrix;
    private Vector2 _vector;

    public Matrix2x2Benchmark()
    {
        _matrix = new Matrix2x2(1, 2, 3, 4);
        _vector = new Vector2(5, 6);
    }

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
            _ = AddExporter(HtmlExporter.Default, MarkdownExporter.GitHub);
            _ = AddLogger(BenchmarkDotNet.Loggers.ConsoleLogger.Default);
            SummaryStyle = SummaryStyle.Default.WithRatioStyle(RatioStyle.Trend);
        }
    }
}
