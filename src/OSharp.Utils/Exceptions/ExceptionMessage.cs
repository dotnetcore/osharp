// -----------------------------------------------------------------------
//  <copyright file="ExceptionMessage.cs" company="OSharp开源团队">
//      Copyright (c) 2014 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2014-10-15 5:28</last-date>
// -----------------------------------------------------------------------

using System;
using System.Text;


namespace OSharp.Exceptions
{
    /// <summary>
    /// 异常信息封装类
    /// </summary>
    public class ExceptionMessage
    {
        #region 字段

        #endregion

        #region 构造函数

        /// <summary>
        /// 以自定义用户信息和异常对象实例化一个异常信息对象
        /// </summary>
        /// <param name="e">异常对象</param>
        /// <param name="userMessage">自定义用户信息</param>
        /// <param name="isHideStackTrace">是否隐藏异常堆栈信息</param>
        public ExceptionMessage(Exception e, string userMessage = null, bool isHideStackTrace = false)
        {
            UserMessage = string.IsNullOrEmpty(userMessage) ? e.Message : userMessage;

            StringBuilder sb = new StringBuilder();
            ExMessage = string.Empty;
            int count = 0;
            string appString = string.Empty;
            while (e != null)
            {
                if (count > 0)
                {
                    appString += "    ";
                }
                ExMessage = e.Message;
                sb.AppendLine(appString + "Message: " + e.Message);
                sb.AppendLine(appString + "Type: " + e.GetType().FullName);
                sb.AppendLine(appString + "Method: " + (e.TargetSite == null ? null : e.TargetSite.Name));
                sb.AppendLine(appString + "Source: " + e.Source);
                if (!isHideStackTrace && e.StackTrace != null)
                {
                    sb.AppendLine(appString + "StackTrace: " + e.StackTrace);
                }
                if (e.InnerException != null)
                {
                    sb.AppendLine(appString + "InnerException: ");
                    count++;
                }
                e = e.InnerException;
            }
            ErrorDetails = sb.ToString();
            sb.Clear();
        }

        #region 属性

        /// <summary>
        /// 用户信息，用于报告给用户的异常消息
        /// </summary>
        public string UserMessage { get; set; }

        /// <summary>
        /// 直接的Exception异常信息，即e.Message属性值
        /// </summary>
        public string ExMessage { get; private set; }

        /// <summary>
        /// 异常输出的详细描述，包含异常消息，规模信息，异常类型，异常源，引发异常的方法及内部异常信息
        /// </summary>
        public string ErrorDetails { get; private set; }

        #endregion

        #region 方法

        /// <summary>
        /// 返回表示当前 <see cref="T:System.Object"/> 的 <see cref="T:System.String"/>。
        /// </summary>
        /// <returns>
        /// <see cref="T:System.String"/>，表示当前的 <see cref="T:System.Object"/>。
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return ErrorDetails;
        }

        #endregion

        #endregion
    }
}