using System;
using Xunit;


namespace OSharp.Exceptions.Tests
{
    public class ExceptionExtensionsTests
    {
        [Fact()]
        public void FormatMessageTest()
        {
            Exception ex = null;
            try
            {
                int num = 0;
                num = 1 / num;
            }
            catch (DivideByZeroException e)
            {
                ex = new OsharpException("服务器正忙，请稍候再尝试。", e);
            }
            Assert.True(ex != null);
            string msg = ex.FormatMessage();
            Assert.Contains("内部异常", msg);
            Assert.Contains("服务器正忙", msg);
        }
    }
}