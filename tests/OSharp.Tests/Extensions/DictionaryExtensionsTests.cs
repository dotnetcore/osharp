using System;
using System.Collections.Generic;

using Shouldly;

using Xunit;


namespace OSharp.Extensions.Tests
{
    public class DictionaryExtensionsTests
    {
        [Fact]
        public void GetOrDefault_Test()
        {
            IDictionary<int, Guid> dict = new Dictionary<int, Guid>()
            {
                { 1, new Guid("2907D385-5BCB-45B7-AB4A-962C3D709645") }
            };
            dict.GetOrDefault(1).ToString().ToUpper().ShouldBe("2907D385-5BCB-45B7-AB4A-962C3D709645");
            dict.GetOrDefault(2).ShouldBe(Guid.Empty);
        }

        [Fact]
        public void GetOrAdd_Test()
        {
            IDictionary<int, Guid> dict = new Dictionary<int, Guid>()
            {
                { 1, new Guid("2907D385-5BCB-45B7-AB4A-962C3D709645") }
            };
            dict.GetOrDefault(1).ToString().ToUpper().ShouldBe("2907D385-5BCB-45B7-AB4A-962C3D709645");
            dict.GetOrAdd(2, () => new Guid("A420DA61-C082-4DA8-BA51-DF185E8DB546")).ToString().ToUpper().ShouldBe("A420DA61-C082-4DA8-BA51-DF185E8DB546");
        }
    }
}
