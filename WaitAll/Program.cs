using System;
using System.Threading;
using ConsoleCopyPaste;

namespace WaitAll
{
    class Program
    {
        static void Main(string[] args)
        {
            TaskDelegate[] delegates =
            {
                () => Console.WriteLine(1), () => Console.WriteLine(2), () => Console.WriteLine(3),
                () => Console.WriteLine(4),() => Console.WriteLine(5),() => Console.WriteLine(6),() => Console.WriteLine(7)
            };
            Parallel.WaitAll(delegates);
            Console.WriteLine("This line must be the last one");
        }

        static class Parallel
        {
            public static void WaitAll(TaskDelegate[] delegates)
            {
                var taskQueue = new TaskQueue();
                var delegatesCount = delegates.Length;
                var counter = 0;
                for (var i = 0; i < delegates.Length; i++)
                {
                    delegates[i] = delegates[i] + (() => counter++);
                    taskQueue.EnqueueTask(delegates[i]);
                }

                while (counter != delegatesCount)
                {
                    Thread.Yield();
                }
                taskQueue.Dispose();
            }
        }
    }
}