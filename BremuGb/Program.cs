using BremuGb.Cpu;
using BremuGb.Memory;
using System;
using System.Diagnostics;

namespace bremugb.core
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            ICpuCore cpuCore = new CpuCore(new MainMemory(), new CpuState());

            Stopwatch stopWatch = new Stopwatch();
            if (!Stopwatch.IsHighResolution)
                throw new InvalidOperationException("No high res timer available in system!");

            stopWatch.Start();

            var multiplier = 10000;
            var cycle = 0;

            var cycles = 17556 * multiplier;
            while (cycle++ < cycles)
            {                
                cpuCore.ExecuteCpuCycle();
            }

            stopWatch.Stop();

            Console.WriteLine($"RunTime: {100*stopWatch.ElapsedMilliseconds/(16.74*multiplier)}%");
        }
    }
}
