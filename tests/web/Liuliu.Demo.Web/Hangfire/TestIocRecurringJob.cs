
using System.Linq;

using Liuliu.Demo.Identity;

using OSharp.Hangfire;


namespace Liuliu.Demo.Web.Hangfire
{
    public class TestIocRecurringJob : RecurringJobBase
    {
        private readonly IIdentityContract _identityContract;

        /// <summary>
        /// 初始化一个<see cref="TestIocRecurringJob"/>类型的新实例
        /// </summary>
        public TestIocRecurringJob(IIdentityContract identityContract)
        {
            _identityContract = identityContract;
        }

        #region Overrides of RecurringJobBase

        /// <summary>
        /// 重写以实现重复作业委托
        /// </summary>
        public override object ExecuteAction()
        {
            return _identityContract.Users.Count();
        }

        #endregion
    }
}
