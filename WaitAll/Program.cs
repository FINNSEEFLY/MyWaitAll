using System;
using System.Threading;
using ConsoleCopyPaste;

namespace WaitAll
{
    class Program
    {
        static void Main(string[] args)
        {
            var delegates = new TaskDelegate[30];
            for (var i = 0; i < 30; i++)
            {
                var i1 = i;
                delegates[i] = () => Console.WriteLine(i1 + 1);
            }
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
                    delegates[i] = delegates[i] + (() => Interlocked.Increment(ref counter));
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