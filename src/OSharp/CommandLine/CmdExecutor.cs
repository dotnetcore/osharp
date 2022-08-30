// -----------------------------------------------------------------------
//  <copyright file="CmdExecutor.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-16 22:00</last-date>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
#endif
using OSharp.Data;


namespace OSharp.CommandLine
{
    /// <summary>
    /// CMD命令执行者
    /// </summary>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]
#endif
    public static class CmdExecutor
    {
        /// <summary>
        /// 执行CMD命令，并获取控制台输出结果
        /// </summary>
        /// <param name="command">CMD命令字符串</param>
        /// <returns></returns>
        public static string ExecuteCmd(string command)
        {
            ProcessStartInfo info = new ProcessStartInfo("cmd", "/c" + command);
            info.RedirectStandardOutput = true;
            info.UseShellExecute = false;
            info.CreateNoWindow = true;
            Process process = new Process();
            process.StartInfo = info;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            return output;
        }

        /// <summary>
        /// 执行CMD命令文件，并获取控制台输出结果
        /// </summary>
        public static string ExecuteCmdFile(string fileName)
        {
            Check.FileExists(fileName, nameof(fileName));
            string ext = Path.GetExtension(fileName);
            if (!new[] { ".bat", ".cmd" }.Any(m => m.Equals(ext, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException("文件名必须以“.bat，.cmd”结尾");
            }

            Process process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.FileName = fileName;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output;
        }
    }
}
