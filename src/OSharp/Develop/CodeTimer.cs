using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;


namespace OSharp.Develop
{
    /// <summary>
    /// 代码性能测试计时器（来自博客园-老赵）
    /// </summary>
    public static class CodeTimer
    {
        #region 私有方法

        /// <summary>
        /// 获取当前CPU循环次数
        /// </summary>
        /// <returns> </returns>
        private static ulong GetCycleCount()
        {
            ulong cycleCount = 0;
            NativeMethods.QueryThreadCycleTime(NativeMethods.GetCurrentThread(), ref cycleCount);
            return cycleCount;
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 计时器初始化 对计时器进行初始化操作，同时对后续操作进行预热，以避免初次操作带来的性能影响
        /// </summary>
        public static void Initialize()
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            Time(string.Empty, 1, () => { });
        }

        /// <summary>
        /// 计时器，传入操作标识名，重复次数，操作过程获取操作的性能数据
        /// </summary>
        /// <param name="name"> 操作标识名 </param>
        /// <param name="iteration"> 重复次数 </param>
        /// <param name="action"> 操作过程的Action </param>
        public static void TimeConsole(string name, int iteration, Action action)
        {
            string output = Time(name, iteration, action);
            Console.WriteLine(output);
        }

        /// <summary>
        /// 计时器，传入操作标识名，重复次数，操作过程获取操作的性能数据
        /// </summary>
        /// <param name="name"> 操作标识名 </param>
        /// <param name="iteration"> 重复次数 </param>
        /// <param name="action"> 操作过程的Action </param>
        public static string Time(string name, int iteration, Action action)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            StringBuilder sb = new StringBuilder();

            ConsoleColor currentForeColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            sb.AppendLine(name);

            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            int[] gcCounts = new int[GC.MaxGeneration + 1];
            for (int i = 0; i < GC.MaxGeneration; i++)
            {
                gcCounts[i] = GC.CollectionCount(1);
            }

            Stopwatch watch = new Stopwatch();
            watch.Start();
            ulong cycleCount = GetCycleCount();
            for (int i = 0; i < iteration; i++)
            {
                action();
            }
            ulong cpuCycles = GetCycleCount() - cycleCount;
            watch.Stop();

            Console.ForegroundColor = currentForeColor;
            sb.AppendLine("\tTime Elapsed:\t" + watch.Elapsed.TotalMilliseconds + "ms");
            sb.AppendLine("\tCPU Cycles:\t" + cpuCycles.ToString("N0"));

            for (int i = 0; i < GC.MaxGeneration; i++)
            {
                int count = GC.CollectionCount(i) - gcCounts[i];
                sb.AppendLine("\tGen" + i + "\t\t" + count);
            }

            sb.AppendLine();

            return sb.ToString();
        }

        #endregion
    }


    internal static class NativeMethods
    {
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool QueryThreadCycleTime(IntPtr threadHandle, ref ulong cycleTime);

        [DllImport("kernel32.dll")]
        internal static extern IntPtr GetCurrentThread();

    }

}
