// -----------------------------------------------------------------------
//  <copyright file="AbstractBuilder.cs" company="OSharp开源团队">
//      Copyright (c) 2014 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2014:07:05 2:53</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

using Xunit;

using OSharp.Reflection;
using OSharp.UnitTest.Infrastructure;


namespace OSharp.Reflection.Tests
{

    public class TypeExtensionsTests
    {
        [Fact()]
        public void IsNullableTypeTest()
        {
            // ReSharper disable ConvertNullableToShortForm
            Assert.True(typeof(int?).IsNullableType());
            Assert.True(typeof(Nullable<int>).IsNullableType());

            Assert.False(typeof(int).IsNullableType());
        }

        [Fact()]
        public void IsEnumerableTest()
        {
            Assert.True(typeof(string[]).IsEnumerable());
            Assert.True(typeof(ICollection<string>).IsEnumerable());
            Assert.True(typeof(IEnumerable<string>).IsEnumerable());
            Assert.True(typeof(IList<string>).IsEnumerable());
            Assert.True(typeof(Hashtable).IsEnumerable());
            Assert.True(typeof(HashSet<string>).IsEnumerable());

            Assert.False(typeof(int).IsEnumerable());
            Assert.False(typeof(string).IsEnumerable());
        }

        [Fact()]
        public void GetNonNummableType()
        {
            Assert.Equal(typeof(int?).GetNonNummableType(), typeof(int));
            Assert.Equal(typeof(Nullable<int>).GetNonNummableType(), typeof(int));

            Assert.Equal(typeof(int).GetNonNummableType(), typeof(int));
        }

        [Fact()]
        public void GetUnNullableTypeTest()
        {
            Assert.Equal(typeof(int?).GetUnNullableType(), typeof(int));
            Assert.Equal(typeof(Nullable<int>).GetUnNullableType(), typeof(int));

            Assert.Equal(typeof(int).GetUnNullableType(), typeof(int));
        }

        [Fact()]
        public void ToDescriptionTest()
        {
            Type type = typeof(TestEntity);
            Assert.Equal(type.ToDescription(), "测试实体");
            PropertyInfo property = type.GetProperty("Id");
            Assert.Equal(property.ToDescription(), "编号");

            type = GetType();
            Assert.Equal(type.ToDescription(), "OSharp.Reflection.Tests.TypeExtensionsTests");
        }

        [Fact()]
        public void HasAttributeTest()
        {
            Type type = GetType();
            MethodInfo method = type.GetMethod("HasAttributeTest");
            Assert.True(method.HasAttribute<FactAttribute>());
        }

        [Fact()]
        public void GetAttributeTest()
        {
            Type type = typeof(TestEntity);
            Assert.Equal(type.GetAttribute<DescriptionAttribute>().Description, "测试实体");
            PropertyInfo property = type.GetProperty("Id");
            Assert.Equal(property.GetAttribute<DescriptionAttribute>().Description, "编号");
            MethodInfo method = GetType().GetMethod("GetAttributeTest");
            Assert.False(method.GetAttribute<FactAttribute>() == null);
        }

        [Fact()]
        public void GetAttributesTest()
        {
            Type type = GetType();
            Assert.Equal(type.GetAttributes<DescriptionAttribute>().Length, 0);
        }

        [Fact()]
        public void IsGenericAssignableFromTest()
        {
            Assert.True(typeof(IEnumerable<>).IsGenericAssignableFrom(typeof(List<>)));
            Assert.True(typeof(List<>).IsGenericAssignableFrom(typeof(List<string>)));

            Assert.Throws<ArgumentException>(() =>
                (typeof(string)).IsGenericAssignableFrom(typeof(int)));
        }

        [Fact()]
        public void IsBaseOnTest()
        {
            Assert.True(typeof(List<>).IsBaseOn(typeof(List<>)));
            Assert.True(typeof(List<>).IsBaseOn(typeof(IList<>)));
            Assert.True(typeof(List<string>).IsBaseOn(typeof(List<string>)));
            Assert.True(typeof(List<string>).IsBaseOn(typeof(IList<string>)));

            Assert.True(typeof(string).IsBaseOn<IEnumerable>());
        }
    }
}