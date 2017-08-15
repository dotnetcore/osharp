using System;
using System.Diagnostics;
using System.Threading;


namespace OSharp.Develop
{
    /// <summary>
    /// 代码性能测试内存计算工具
    /// </summary>
    public static class CodeRamer
    {
        /// <summary>
        /// 内存计算初始化，同时后续操作进行预热，以避免初次操作带来的性能影响
        /// </summary>
        public static void Initialize()
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            Ram(string.Empty, () => { });
        }

        /// <summary>
        /// 内存计算，传入操作标识名，重复次数，操作过程获取操作的性能数据
        /// </summary>
        /// <param name="name"> 操作标识名 </param>
        /// <param name="action"> 操作过程的Action </param>
        public static void Ram(string name, Action action)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            ConsoleColor currentForeColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(name);

            GC.Collect();
            long start = GC.GetTotalMemory(true);
            action();
            GC.Collect();
            GC.WaitForFullGCComplete();
            long end = GC.GetTotalMemory(true);

            Console.ForegroundColor = currentForeColor;
            long result = end - start;
            Console.WriteLine("\tRam:\t" + result + " B");
            Console.WriteLine("\tRam:\t" + result / 1024 + " KB");
            Console.WriteLine("\tRam:\t" + result / 1024 / 1024 + " MB");
            Console.WriteLine();
        }
    }
}
