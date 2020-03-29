using System;

using Shouldly;

using Xunit;

namespace OSharp.Develop.Tests
{
    public class CodeTimerTests
    {
        [Fact]
        public void Time_Test()
        {
            CodeTimer.Initialize();
            string output = CodeTimer.Time("name", 10000, () =>
            {
                int sum = 0;
                for (int i = 1; i <= 100; i++)
                {
                    sum++;
                }
                sum.ShouldBe(100);
            });
            output.ShouldContain("CPU Cycles");
            output.ShouldContain("ms");
        }
    }
}
