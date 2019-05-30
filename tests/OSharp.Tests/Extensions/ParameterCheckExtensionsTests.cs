using System;

using Xunit;


namespace OSharp.Extensions.Tests
{

    public class ParameterCheckExtensionsTests
    {
        [Fact()]
        public void RequiredTest()
        {
            Assert.Throws<Exception>(() => "abc".Required(str => str.Length > 3, "message"));
            "abc".Required(str => str.Length == 3, "message");
        }

        [Fact()]
        public void CheckNotNullTest()
        {
            Assert.Throws<ArgumentNullException>(() => ((object)null).CheckNotNull("param"));
            new object().CheckNotNull("value");
        }

        [Fact()]
        public void CheckNotEmptyTest()
        {
            Assert.Throws<ArgumentException>(() => Guid.Empty.CheckNotEmpty("param"));
            Guid.NewGuid().CheckNotEmpty("guid");
        }

        [Fact()]
        public void CheckNotNullOrEmptyTest_String()
        {
            Assert.Throws<ArgumentNullException>(() => ((string)null).CheckNotNullOrEmpty("param"));
            Assert.Throws<ArgumentException>(() => string.Empty.CheckNotNullOrEmpty("param"));
            "abc".CheckNotNullOrEmpty("param");
        }

        [Fact()]
        public void CheckNotNullOrEmptyTest_Collection()
        {
            Assert.Throws<ArgumentNullException>(() => ((object[])null).CheckNotNullOrEmpty("param"));
            Assert.Throws<ArgumentException>(() => new object[] { }.CheckNotNullOrEmpty("param"));
            new object[] { "abc" }.CheckNotNullOrEmpty("param");
        }

        [Fact()]
        public void CheckLessThanTest()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => 5.CheckLessThan("param", 4));
            Assert.Throws<ArgumentOutOfRangeException>(() => 5.CheckLessThan("param", 5));
            Assert.Throws<ArgumentOutOfRangeException>(() => 5.CheckLessThan("param", 4, true));
            5.CheckLessThan("param", 6);
            5.CheckLessThan("param", 5, true);
        }

        [Fact()]
        public void CheckGreaterThanTest()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => 5.CheckGreaterThan("param", 6));
            Assert.Throws<ArgumentOutOfRangeException>(() => 5.CheckGreaterThan("param", 5));
            Assert.Throws<ArgumentOutOfRangeException>(() => 5.CheckGreaterThan("param", 6, true));
            5.CheckGreaterThan("param", 4);
            5.CheckGreaterThan("param", 5, true);
        }

        [Fact()]
        public void CheckBetweenTest()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => 5.CheckBetween("param", 1, 4));
            Assert.Throws<ArgumentOutOfRangeException>(() => 5.CheckBetween("param", 6, 9));
            Assert.Throws<ArgumentOutOfRangeException>(() => 5.CheckBetween("param", 1, 5));
            Assert.Throws<ArgumentOutOfRangeException>(() => 5.CheckBetween("param", 5, 9));
            5.CheckBetween("param", 1, 9);
            5.CheckBetween("param", 1, 5, false, true);
            5.CheckBetween("param", 5, 9, true, false);
            5.CheckBetween("param", 5, 5, true, true);
        }        
    }
}
