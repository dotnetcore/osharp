// -----------------------------------------------------------------------
//  <copyright file="Output.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-10-29 10:44</last-date>
// -----------------------------------------------------------------------

using System;
using System.Threading.Tasks;

using OSharp.Data;


namespace OSharp.Wpf.Data
{
    /// <summary>
    /// 信息输出类 
    /// </summary>
    public static class Output
    {
        /// <summary>
        /// 获取或设置 状态栏输出委托
        /// </summary>
        public static Action<string> StatusBar = msg => throw new InvalidOperationException("Output is not initialized");

        /// <summary>
        /// 带倒计时的状态栏信息输出
        /// </summary>
        /// <param name="format">字符串格式，必须包含秒的{0}格式</param>
        /// <param name="countdownSeconds">总计时秒数</param>
        /// <returns></returns>
        public static async Task StatusBarCountdown(string format, int countdownSeconds)
        {
            Check.Required(format, msg=> msg != null && msg.Contains("{0}"), "format格式不正确，必须包含{0}");

            if (countdownSeconds == 0)
            {
                return;
            }
            for (int i = countdownSeconds; i > 0; i--)
            {
                string msg = string.Format(format, i);
                StatusBar(msg);
                await Task.Delay(1000);
            }
        }
    }
}