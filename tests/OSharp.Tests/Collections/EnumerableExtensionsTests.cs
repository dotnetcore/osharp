using OSharp.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using OSharp.Exceptions;
using OSharp.Filter;
using OSharp.UnitTest.Infrastructure;

using Xunit;


namespace OSharp.Collections.Tests
{
    public class EnumerableExtensionsTests
    {
        [Fact]
        public void ShuffleTest()
        {
            List<int> source = new List<int> { 1, 2, 3, 4, 5, 6 };
            List<int> result = source.Shuffle().ToList();
            Assert.NotEqual(source, result);
        }

        [Fact()]
        public void ExpandAndToStringTest()
        {
            List<int> source = new List<int>();
            //当为空集合时，返回null
            Assert.Equal(source.ExpandAndToString(), string.Empty);

            source.AddRange(new List<int>() { 1, 2, 3, 4, 5, 6 });
            //没有分隔符时，默认为逗号
            Assert.Equal("1,2,3,4,5,6", source.ExpandAndToString());
            Assert.Equal("123456", source.ExpandAndToString(null));
            Assert.Equal("123456", source.ExpandAndToString(""));
            Assert.Equal("1|2|3|4|5|6", source.ExpandAndToString("|"));
        }

        [Fact()]
        public void ExpandAndToStringTest2()
        {
            List<int> source = new List<int> { 1, 2, 3, 4, 5, 6 };

            //转换委托不能为空
            Assert.Throws<ArgumentNullException>(() => source.ExpandAndToString(itemFormatFunc: null));
            //没有分隔符时，默认为逗号
            Assert.Equal("2,3,4,5,6,7", source.ExpandAndToString(item => (item + 1).ToString()));
            Assert.Equal("2|3|4|5|6|7", source.ExpandAndToString(item => (item + 1).ToString(), "|"));
        }

        [Fact()]
        public void IsEmptyTest()
        {
            List<int> source = new List<int>();
            Assert.True(source.IsEmpty());

            source.Add(1);
            Assert.False(source.IsEmpty());
        }

        [Fact()]
        public void WhereIfTest()
        {
            List<int> source = new List<int> { 1, 2, 3, 4, 5, 6, 7 };
            Assert.Equal(source.WhereIf(m => m > 5, false).ToList(), source);
            List<int> actual = new List<int> { 6, 7 };
            Assert.Equal(source.WhereIf(m => m > 5, true).ToList(), actual);
        }

        [Fact()]
        public void DistinctByTest()
        {
            List<int> source = new List<int> { 1, 2, 3, 3, 4, 4, 5, 6, 7, 7 };
            List<int> actual = new List<int> { 1, 2, 3, 4, 5, 6, 7 };
            Assert.Equal(source.DistinctBy(m => m).ToList(), actual);
        }

        [Fact()]
        public void OrderByTest()
        {
            IEnumerable<TestEntity> source = new List<TestEntity>
            {
                new TestEntity { Id = 1, Name = "abc" },
                new TestEntity { Id = 4, Name = "fda", IsDeleted = true },
                new TestEntity { Id = 6, Name = "rwg", IsDeleted = true },
                new TestEntity { Id = 3, Name = "hdg" },
            };

            Assert.Equal("hdg", source.OrderBy("Id").ToArray()[1].Name);
            Assert.Equal(1, source.OrderBy("Name", ListSortDirection.Descending).ToArray()[3].Id);
            Assert.Equal("hdg", source.OrderBy(new SortCondition("Id")).ToArray()[1].Name);
            Assert.Equal("hdg", source.OrderBy(new SortCondition<TestEntity>(m => m.Id)).ToArray()[1].Name);
            Assert.Equal("fda", source.OrderBy(new SortCondition<TestEntity>(m => m.Name.Length)).ToArray()[1].Name);
            Assert.Equal(1, source.OrderBy(new SortCondition("Name", ListSortDirection.Descending)).ToArray()[3].Id);
        }

        [Fact()]
        public void ThenByTest()
        {
            IEnumerable<TestEntity> source = new List<TestEntity>
            {
                new TestEntity { Id = 1, Name = "abc" },
                new TestEntity { Id = 4, Name = "fda", IsDeleted = true },
                new TestEntity { Id = 6, Name = "rwg", IsDeleted = true },
                new TestEntity { Id = 3, Name = "hdg" },
            };
            Assert.Equal("fda", source.OrderBy("IsDeleted").ThenBy("Id").ToArray()[2].Name);
            Assert.Equal("hdg", source.OrderBy("IsDeleted", ListSortDirection.Descending).ThenBy("Id", ListSortDirection.Descending).ToArray()[2].Name);
            Assert.Equal("fda", source.OrderBy(new SortCondition("IsDeleted")).ThenBy(new SortCondition("Name")).ToArray()[2].Name);
        }

        [Fact()]
        public void AssertTest()
        {
            var list = new List<int>(){ 1, 2, 3, 4, 5, 6, 7 };
            Assert.Throws<InvalidOperationException>(() => list.Assert(m => m < 6).ToArray());
            Assert.Throws<OsharpException>(() => list.Assert(m => m < 6, m => new OsharpException()).ToArray());
            list.Assert(m => m <= 7).ToArray();
        }
    }
}
