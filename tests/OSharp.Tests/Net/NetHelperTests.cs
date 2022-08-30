using System.Runtime.Versioning;

using Xunit;

namespace OSharp.Net.Tests
{
    [SupportedOSPlatform("windows")]
    public class NetHelperTests
    {
        [Fact()]
        public void PingTest()
        {
            bool flag = NetHelper.Ping("localhost");
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
