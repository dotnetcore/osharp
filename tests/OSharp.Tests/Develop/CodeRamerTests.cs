using System;

using Xunit;


namespace OSharp.Develop.Tests
{
    public class CodeRamerTests
    {
        [Fact]
        public void Ram_Test()
        {
            CodeRamer.Initialize();
            CodeRamer.Ram("name",
                () =>
                {
                    int sum = 0;
                    for (int i = 1; i <= 10000; i++)
                    {
                        sum += i;
                    }
                    Console.WriteLine($"sum: {sum}");
                });
        }
    }
}
