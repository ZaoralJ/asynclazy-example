namespace AsyncLazy.Example
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;

    internal static class Program
    {
        internal static void Main()
        {
            Test01();
            Console.WriteLine();
            Test02();
            Console.WriteLine();
            Test03();
        }

        private static void Test01()
        {
            Console.WriteLine("// Test 01");
            Console.WriteLine();
            Console.WriteLine("Creating object with async lazy properties.");
            var sw = new Stopwatch();
            sw.Start();
            var o = new ObjectWithLazy(false);
            ShowElapsed(sw.ElapsedMilliseconds);

            Console.WriteLine("Waiting 1s");
            Thread.Sleep(1000);
            ShowElapsed(sw.ElapsedMilliseconds);

            sw.Restart();
            Console.WriteLine("First call, should take 3s (1.5s for each)");
            Console.WriteLine($"Result: {o.Number1 + o.Number2}");
            ShowElapsed(sw.ElapsedMilliseconds);
            sw.Restart();
            Console.WriteLine("Second call, should take 0");
            Console.WriteLine($"Result: {o.Number1 + o.Number2}");
            ShowElapsed(sw.ElapsedMilliseconds);

            Console.WriteLine();

            Console.WriteLine("Creating object with async lazy properties and start them.");
            sw.Restart();
            o = new ObjectWithLazy(true);
            ShowElapsed(sw.ElapsedMilliseconds);

            Console.WriteLine("Waiting 1s");
            Thread.Sleep(1000);
            ShowElapsed(sw.ElapsedMilliseconds);
            sw.Restart();

            Console.WriteLine("First call, should take cca 0.5s, because lazy initialization has been start in ctor.");
            Console.WriteLine($"Result: {o.Number1 + o.Number2}");
            ShowElapsed(sw.ElapsedMilliseconds);
            sw.Restart();
            Console.WriteLine("Second call, should take 0");
            Console.WriteLine($"Result: {o.Number1 + o.Number2}");
            ShowElapsed(sw.ElapsedMilliseconds);
        }

        private static void Test02()
        {
            Console.WriteLine("// Test 02");
            Console.WriteLine("// 50 object with started lazy in parallel");
            Console.WriteLine();
            var sw = new Stopwatch();
            sw.Start();

            Enumerable.Range(0, 50).AsParallel().ForAll(i =>
            {
                var o = new ObjectWithLazy(true);
                Console.WriteLine($"Iteration: {i}, Result: {o.Number1 + o.Number2}, ThreadId: {Thread.CurrentThread.ManagedThreadId}");
            });

            sw.Stop();
            ShowElapsed(sw.ElapsedMilliseconds);
        }

        private static void Test03()
        {
            Console.WriteLine("// Test 03");
            Console.WriteLine("// 50 object with started lazy, sync");
            Console.WriteLine();
            var sw = new Stopwatch();
            sw.Start();

            var list = Enumerable.Range(0, 50).Select(o => new ObjectWithLazy(true)).ToList();

            list.ForEach(o => Console.WriteLine($"Result: {o.Number1 + o.Number2}, ThreadId: {Thread.CurrentThread.ManagedThreadId}"));

            sw.Stop();
            ShowElapsed(sw.ElapsedMilliseconds);
        }

        private static void ShowElapsed(long ms) => Console.WriteLine($"Elapsed ms: {ms}");
    }
}
