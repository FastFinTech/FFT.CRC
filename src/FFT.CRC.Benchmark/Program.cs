using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace FFT.CRC.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<PerformanceCounter>();
        }
    }

    //[SimpleJob(RuntimeMoniker.NetCoreApp31)]
    //[SimpleJob(RuntimeMoniker.NetCoreApp50)]
    public class PerformanceCounter
    {
        byte[][] spans;


        [GlobalSetup]
        public void Setup()
        {
            this.spans = new[] {
                new byte[] {1,2,3},
                new byte[] {4,5,6},
                new byte[] {7,8,9},
            };
        }

        [Benchmark]
        public uint SpeedTest()
        {
            var builder = CRC32Builder.InitializedValue;
            for (var i = 0; i < 1024; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    builder.Add(this.spans[j]);
                }
            }
            return builder.Value;
        }
    }
}
