using System;

using Hangfire;

using OSharp.Hangfire;


namespace Liuliu.Demo.Web.Startups
{
    public class TestHangfireJob : RecurringJobBase
    {
        #region Overrides of RecurringJobBase

        /// <summary>
        /// 获取或设置 重复执行时间的CRON表达式
        /// </summary>
        public override string CronExpression => Cron.Minutely();

        /// <summary>
        /// 重写以实现重复作业委托
        /// </summary>
        public override object ExecuteAction()
        {
            return $"Now Date: {DateTime.Now}";
        }
        
        #endregion
    }
}
