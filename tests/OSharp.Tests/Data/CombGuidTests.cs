using System;
using System.Collections.Generic;
using System.Linq;

using Shouldly;

using Xunit;


namespace OSharp.Data.Tests
{
    public class CombGuidTests
    {
        [Fact]
        public void NewGuid_Test()
        {
            List<Guid> ids = new List<Guid>();
            for (int i = 0; i < 100; i++)
            {
                ids.Add(CombGuid.NewGuid());
            }
            ids.Distinct().Count().ShouldBe(100);
        }
    }
}
