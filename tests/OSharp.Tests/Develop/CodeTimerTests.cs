using System;

using Xunit;

namespace OSharp.Develop.Tests
{
    public class CodeTimerTests
    {
        [Fact]
        public void Time_Test()
        {
            CodeTimer.Initialize();
            CodeTimer.Time("name", 10000, () =>
            {
                int sum = 0;
                for (int i = 1; i <= 100; i++)
                {
                    sum++;
                }
                Console.WriteLine(sum);
            });
        }
    }
}
