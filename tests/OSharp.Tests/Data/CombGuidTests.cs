using System;

using Shouldly;

using Xunit;


namespace OSharp.Data.Tests
{
    public class CombGuidTests
    {
        [Fact]
        public void NewGuid_Test()
        {
            Guid id = CombGuid.NewGuid();
            DateTime dt = CombGuid.GetDateFrom(id);
            DateTime.Now.Subtract(dt).ShouldBeLessThan(TimeSpan.FromSeconds(1));
        }
    }
}
