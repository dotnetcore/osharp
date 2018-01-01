using System;
using System.Collections.Generic;
using System.Text;

using Shouldly;

using Xunit;


namespace OSharp.Dependency.Tests
{
    public class ServiceLocatorTests
    {
        [Fact]
        public void Instance_Test()
        {
            ServiceLocator locator1 = ServiceLocator.Instance;
            ServiceLocator locator2 = ServiceLocator.Instance;
            locator2.ShouldBe(locator1);
        }
    }
}
