using System;
using System.Runtime.Versioning;

using Shouldly;

using Xunit;


namespace OSharp.Develop.Tests
{
    [SupportedOSPlatform("windows")]
    public class CodeRamerTests
    {
        [Fact]
        public void Ram_Test()
        {
            CodeRamer.Initialize();
            string output = CodeRamer.Ram("name",
                () =>
                {
                    int sum = 0;
                    for (int i = 1; i <= 10000; i++)
                    {
                        sum += i;
                    }
                    sum.ShouldBe(50005000);
                });
            output.ShouldContain("\tRam:\t");
            output.ShouldContain("KB");
            output.ShouldContain("MB");
        }
    }
}
