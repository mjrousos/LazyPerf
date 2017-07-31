using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace LazyPerf
{
    class Program
    {
        static readonly object syncObject = new object();
        static readonly Stopwatch stopwatch = new Stopwatch();
        const int iterations = 10000000;

        static void Main(string[] args)
        {
            Console.WriteLine("Lazy<T> Perf Tester");
            Console.WriteLine("-------------------");
            Console.WriteLine();

            Time(CreateWithLazyDefaultCtor, nameof(CreateWithLazyDefaultCtor), iterations);
            Time(CreateWithLazyFactory, nameof(CreateWithLazyFactory), iterations);
            Time(CreateWithDoubleCheckedLocking, nameof(CreateWithDoubleCheckedLocking), iterations);
        }

        private static void Time(Action targetMethod, string methodName, int iters)
        {
            // Run once so we're not cold
            targetMethod.Invoke();

            Console.Write($"Running {methodName} {iters} times... ");
            stopwatch.Start();
            for (int i = 0; i < iters; i++)
            {
                targetMethod.Invoke();
            }
            stopwatch.Stop();
            Console.WriteLine($"{stopwatch.ElapsedMilliseconds} ms");
            stopwatch.Reset();
        }

        private static void CreateWithLazyDefaultCtor()
        { 
            var lazyVar = new Lazy<List<int>>(); 
            var count = lazyVar.Value.Count; 
        }

        private static void CreateWithLazyFactory()
        { 
            var lazyVar = new Lazy<List<int>>(() => new List<int>()); 
            var count = lazyVar.Value.Count; 
        }

        private static void CreateWithDoubleCheckedLocking()
        {
            List<int> lazyVar = null;
            if (lazyVar == null)
            {
                lock (syncObject)
                {
                    if (lazyVar == null)
                    {
                        lazyVar = new List<int>();
                    }
                }
            }
            var count = lazyVar.Count;
        }
    }
}
