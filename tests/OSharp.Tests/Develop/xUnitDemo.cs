using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Xunit;


namespace OSharp.Tests.Develop
{
    public class XUnitDemoTests
    {
        [Fact]
        public void TestAdd()
        {
            Assert.Equal(5, Add(2, 3));
            Assert.Equal(7, Add(2, 5));
            Assert.Equal(10, Add(4, 6));
        }

        [Theory]
        [InlineData(5, 2, 3)]
        [InlineData(7, 2, 5)]
        [InlineData(10, 5, 5)]
        public void TestAddUseInlineData(int sum, int a, int b)
        {
            Assert.Equal(sum, Add(a, b));
        }


        int Add(int a, int b)
        {
            return a + b;
        }
    }

    public class TestsUseClassData
    {
        public bool HasMobile(Person person)
        {
            return !string.IsNullOrWhiteSpace(person.Mobile);
        }

        [Theory]
        [ClassData(typeof(DataForTest))]//使用ClassData将数据传递给Theory
        public void IndexOf(Person person, bool hasMobile)
        {
            Assert.Equal(hasMobile, HasMobile(person));
        }
    }
    /// <summary>
    /// 在这里我创建了一个继承自IEnumerable <object []>的类，注意它必须是一个对象，
    /// 否则xUnit会抛出一个错误。
    /// </summary>
    public class DataForTest : IEnumerable<object[]>
    {
        //这里是我们传递给Theory的数据
        private readonly List<object[]> _data = new List<object[]>
        {
            new object[] { new Person() { Id=1,Name="周三"},false },
            new object[] { new Person() { Id=2,Name="王石", Mobile = "139XXXXXXXX"},true },
            new object[] { new Person() { Id=3,Name="赵坎", Mobile = ""},false },
        };

        public IEnumerator<object[]> GetEnumerator()
        { return _data.GetEnumerator(); }

        IEnumerator IEnumerable.GetEnumerator()
        { return GetEnumerator(); }
    }

    public class Person
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Mobile { get; set; }
    }

    public class TestsUseMemberData
    {
        public bool HasMobile(Person person)
        {
            return !string.IsNullOrWhiteSpace(person.Mobile);
        }

        //传递数据给Theory的方法
        public static IEnumerable<object[]> GetTestData()
        {
            yield return new object[] { new Person() { Id = 1, Name = "周三" }, false };
            yield return new object[] { new Person() { Id = 2, Name = "王石", Mobile = "139XXXXXXXX" }, true };
            yield return new object[] { new Person() { Id = 3, Name = "赵坎", Mobile = "" }, false };
        }


        [Theory]
        [MemberData(nameof(GetTestData))]//使用MemberData将数据传递给Theory
        public void IndexOf(Person person, bool hasMobile)
        {
            Assert.Equal(hasMobile, HasMobile(person));
        }
    }
}
