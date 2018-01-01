using System;
using System.Collections.Generic;
using System.Linq;

using Shouldly;

using Xunit;


namespace OSharp.Collections.Tests
{
    public class CollectionExtensionsTests
    {
        [Fact]
        public void AddIfNotExistTest()
        {
            List<int> nums = new List<int>() { 1, 2, 3 };
            nums.AddIfNotExist(2);
            nums.Count(m => m == 2).ShouldBe(1);
            nums.AddIfNotExist(5);
            nums.Count(m => m == 5).ShouldBe(1);

            nums = null;
            Assert.Throws<ArgumentNullException>(()=>
            {
                nums.AddIfNotExist(3);
            });
        }

        [Fact]
        public void AddIfNotNullTest()
        {
            List<string> strs = new List<string>() { "abc", "bcd", "cde" };
            strs.AddIfNotNull(null);
            strs.Count.ShouldBe(3);
            strs.AddIfNotNull("abc");
            strs.Count.ShouldBe(4);

            strs = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                strs.AddIfNotNull("abc");
            });
        }
    }
}
