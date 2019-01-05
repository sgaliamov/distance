using BenchmarkDotNet.Running;

namespace Distance.Benchmarks
{
    internal class Program
    {
        private static void Main()
        {
            BenchmarkRunner.Run<Benchmark>();
        }
    }
}
