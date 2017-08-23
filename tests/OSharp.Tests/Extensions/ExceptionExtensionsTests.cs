using System;
using OSharp.Exceptions;
using Xunit;


namespace OSharp.Extensions.Tests
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
            Assert.True(msg.Contains("内部异常"));
            Assert.True(msg.Contains("服务器正忙"));
        }
    }
}