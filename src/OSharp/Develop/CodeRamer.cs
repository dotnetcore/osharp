using System;
using System.Diagnostics;
using System.Text;
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
        public static void RamConsole(string name, Action action)
        {
            string output = Ram(name, action);
            Console.WriteLine(output);
        }

        /// <summary>
        /// 内存计算，传入操作标识名，重复次数，操作过程获取操作的性能数据
        /// </summary>
        /// <param name="name"> 操作标识名 </param>
        /// <param name="action"> 操作过程的Action </param>
        public static string Ram(string name, Action action)
        {
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            ConsoleColor currentForeColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            sb.AppendLine(name);

            GC.Collect();
            long start = GC.GetTotalMemory(true);
            action();
            GC.Collect();
            GC.WaitForFullGCComplete();
            long end = GC.GetTotalMemory(true);

            Console.ForegroundColor = currentForeColor;
            long result = end - start;

            sb.AppendLine("\tRam:\t" + result + " B");
            sb.AppendLine("\tRam:\t" + result / 1024 + " KB");
            sb.AppendLine("\tRam:\t" + result / 1024 / 1024 + " MB");
            sb.AppendLine();

            return sb.ToString();
        }
    }
}
