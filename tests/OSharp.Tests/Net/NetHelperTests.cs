
using System;

using Xunit;

namespace OSharp.Net.Tests
{
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
            if (!OperatingSystem.IsWindows())
            {
                return;
            }

            bool flag = NetHelper.IsInternetConnected();
            Assert.True(flag);
        }
    }
}
