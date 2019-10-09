using Xunit;

using System;

using OSharp.UnitTest.Infrastructure;


namespace OSharp.Develop.T4.Tests
{
    public class T4ModelInfoTests
    {
        [Fact()]
        public void T4ModelInfoTest()
        {
            //OSharp.UnitTest.Infrastructure
            const string pattern = "(?<=OSharp.).*(?=.Infrastructure)";
            Type type = typeof(TestEntity);
            T4ModelInfo modelInfo = new T4ModelInfo(type, pattern);
            Assert.Equal("UnitTest", modelInfo.ModuleName);
        }
    }
}