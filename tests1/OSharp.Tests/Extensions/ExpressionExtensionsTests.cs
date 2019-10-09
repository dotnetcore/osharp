// -----------------------------------------------------------------------
//  <copyright file="ExpressionExtensionsTests.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-10-14 17:14</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using OSharp.Linq;
using OSharp.UnitTest.Infrastructure;

using Xunit;


namespace OSharp.Extensions.Tests
{
    public class ExpressionExtensionsTests : EntityTestBase
    {
        [Fact()]
        public void ComposeTest()
        {
            Expression<Func<TestEntity, bool>> predicate = m => m.IsDeleted;
            Expression<Func<TestEntity, bool>> p1 = predicate.Compose(m => m.Id > 500, Expression.AndAlso);
            Expression<Func<TestEntity, bool>> p2 = predicate.Compose(m => m.Id > 500, Expression.OrElse);
            List<TestEntity> list1 = Entities.Where(m => m.IsDeleted && m.Id > 500).ToList();
            List<TestEntity> list2 = Entities.Where(p1.Compile()).ToList();
            Assert.True(list1.SequenceEqual(list2));
            list1 = Entities.Where(m => m.IsDeleted || m.Id > 500).ToList();
            list2 = Entities.Where(p2.Compile()).ToList();
            Assert.True(list1.SequenceEqual(list2));
        }

        [Fact()]
        public void AndTest()
        {
            Expression<Func<TestEntity, bool>> predicate = m => m.IsDeleted;
            Expression<Func<TestEntity, bool>> actual = m => m.IsDeleted && m.Id > 500;
            predicate = predicate.And(m => m.Id > 500);
            List<TestEntity> list1 = Entities.Where(predicate.Compile()).ToList();
            List<TestEntity> list2 = Entities.Where(actual.Compile()).ToList();
            Assert.True(list1.SequenceEqual(list2));
        }

        [Fact()]
        public void OrTest()
        {
            Expression<Func<TestEntity, bool>> predicate = m => m.IsDeleted;
            Expression<Func<TestEntity, bool>> actual = m => m.IsDeleted || m.Id > 500;
            predicate = predicate.Or(m => m.Id > 500);
            List<TestEntity> list1 = Entities.Where(predicate.Compile()).ToList();
            List<TestEntity> list2 = Entities.Where(actual.Compile()).ToList();
            Assert.True(list1.SequenceEqual(list2));
        }
    }
}