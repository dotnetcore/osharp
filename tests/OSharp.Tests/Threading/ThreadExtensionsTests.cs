using System.Threading;

using Xunit;


namespace OSharp.Threading.Tests
{
    public class ThreadExtensionsTests
    {
        [Fact()]
        public void CancelSleepTest()
        {
            var thread = new Thread(() =>
            {
                try
                {
                    Thread.Sleep(10000);
                }
                catch (ThreadInterruptedException)
                { }
            });
            thread.Start();
            Thread.Sleep(300);
            Assert.Equal(thread.ThreadState, ThreadState.WaitSleepJoin);
            thread.CancelSleep();
            Thread.Sleep(300);
            Assert.NotEqual(thread.ThreadState, ThreadState.WaitSleepJoin);
        }
    }
}