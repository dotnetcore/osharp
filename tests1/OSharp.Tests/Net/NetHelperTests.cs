using Xunit;

namespace OSharp.Net.Tests
{
    public class NetHelperTests
    {
        [Fact()]
        public void PingTest()
        {
            bool flag = NetHelper.Ping("baidu.com");
            Assert.True(flag);
        }

        [Fact()]
        public void IsInternetConnectedTest()
        {
            bool flag = NetHelper.IsInternetConnected();
            Assert.True(flag);
        }
    }
}