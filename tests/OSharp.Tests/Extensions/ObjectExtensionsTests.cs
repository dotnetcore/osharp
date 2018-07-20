// -----------------------------------------------------------------------
//  <copyright file="AbstractBuilder.cs" company="OSharp开源团队">
//      Copyright (c) 2014 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2014:07:05 3:17</last-date>
// -----------------------------------------------------------------------

using System;

using OSharp.Json;

using Xunit;


namespace OSharp.Extensions.Tests
{

    public class ObjectExtensionsTests
    {
        [Fact()]
        public void CastToTest()
        {
            Assert.Equal(null, ((object)null).CastTo<object>());
            Assert.Equal(123, "123".CastTo<int>());
            Assert.Equal("123", 123.CastTo<string>());
            Assert.Equal("True", true.CastTo<string>());
            Assert.Equal(true, "true".CastTo<bool>());
            Assert.Equal("56D768A3-3D74-43B4-BD7B-2871D675CC4B".CastTo<Guid>(), new Guid("56D768A3-3D74-43B4-BD7B-2871D675CC4B"));
            Assert.Equal(UriKind.Absolute, 1.CastTo<UriKind>());
            Assert.Equal(UriKind.RelativeOrAbsolute, "RelativeOrAbsolute".CastTo<UriKind>());

            Assert.Equal(123, "abc".CastTo<int>(123));

            Assert.Throws<FormatException>(() => "abc".CastTo<int>());
        }

        [Fact()]
        public void IsBetweenTest()
        {
            const int num = 5;

            Assert.True(num.IsBetween(1, 10));
            Assert.True(num.IsBetween(5, 10));
            Assert.True(num.IsBetween(5, 10));
            Assert.True(num.IsBetween(5, 10, true));
            Assert.True(num.IsBetween(0, 5, true, true));
            Assert.True(num.IsBetween(5, 5, true, true));
            Assert.False(num.IsBetween(5, 10, false));
            Assert.False(num.IsBetween(0, 5, true, false));
            Assert.False(num.IsBetween(5, 5, true, false));
            Assert.False(num.IsBetween(5, 5, false, true));
            Assert.False(num.IsBetween(5, 5, false, false));
        }

        [Fact()]
        public void ToDynamicTest()
        {
            var obj1 = new { Name = "GMF" };
            dynamic res1 = obj1.ToDynamic();
            Assert.True(res1.Name == "GMF");
            var obj2 = new { Name = "GMF", Value = new { IsLocked = true } };
            Assert.True(obj2.ToDynamic().Value.IsLocked);
        }

        [Fact()]
        public void ToJsonStringTest()
        {
            Assert.Equal("null", ((object)null).ToJsonString());
            Assert.Equal("\"\"", "".ToJsonString());
            Assert.Equal("123", 123.ToJsonString());
        }
    }
}