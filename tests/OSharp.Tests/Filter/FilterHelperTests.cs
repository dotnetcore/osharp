// -----------------------------------------------------------------------
//  <copyright file="AbstractBuilder.cs" company="OSharp开源团队">
//      Copyright (c) 2014 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2014:07:05 3:24</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using OSharp.Filter;
using OSharp.UnitTest.Infrastructure;

using Xunit;


namespace OSharp.Filter.Tests
{
    public class FilterHelperTests : EntityTestBase
    {
        [Fact()]
        public void GetExpressionTest()
        {
            IQueryable<TestEntity> source = Entities.AsQueryable();

            //空条件
            FilterRule rule = null;
            Expression<Func<TestEntity, bool>> predicate = FilterHelper.GetExpression<TestEntity>(rule);
            Assert.True(source.Where(predicate).SequenceEqual(source));

            //单条件，属性不存在
            rule = new FilterRule("Name1", "5", FilterOperate.EndsWith);
            Assert.Throws<InvalidOperationException>(() => FilterHelper.GetExpression<TestEntity>(rule));

            //单条件
            rule = new FilterRule("Name", "5", FilterOperate.EndsWith);
            predicate = FilterHelper.GetExpression<TestEntity>(rule);
            Assert.True(source.Where(predicate).SequenceEqual(source.Where(m => m.Name.EndsWith("5"))));

            //二级条件
            rule = new FilterRule("Name.Length", 5, FilterOperate.Greater);
            predicate = FilterHelper.GetExpression<TestEntity>(rule);
            Assert.True(source.Where(predicate).SequenceEqual(source.Where(m => m.Name.Length > 5)));

            //多条件，异常
            Assert.Throws<InvalidOperationException>(() => new FilterGroup
            {
                Rules = new List<FilterRule> { rule, new FilterRule("IsDeleted", true) },
                Operate = FilterOperate.Equal
            });

            //多条件
            FilterGroup group = new FilterGroup
            {
                Rules = new List<FilterRule> { rule, new FilterRule("IsDeleted", true) },
                Operate = FilterOperate.And
            };
            predicate = FilterHelper.GetExpression<TestEntity>(group);
            Assert.True(source.Where(predicate).SequenceEqual(source.Where(m => m.Name.Length > 5 && m.IsDeleted)));

            //条件组嵌套
            DateTime dt = DateTime.Now;
            group = new FilterGroup
            {
                Rules = new List<FilterRule>
                {
                    new FilterRule("AddDate", dt, FilterOperate.Greater)
                },
                Groups = new List<FilterGroup> { group },
                Operate = FilterOperate.Or
            };
            predicate = FilterHelper.GetExpression<TestEntity>(group);
            Assert.True(source.Where(predicate).SequenceEqual(source.Where(m => m.AddDate > dt || m.Name.Length > 5 && m.IsDeleted)));

        }

        [Fact()]
        public void ToOperateCodeTest()
        {
            Assert.Equal(FilterOperate.And.ToOperateCode(), "and");
            Assert.Equal(FilterOperate.Or.ToOperateCode(), "or");
            Assert.Equal(FilterOperate.Equal.ToOperateCode(), "equal");
            Assert.Equal(FilterOperate.NotEqual.ToOperateCode(), "notequal");
            Assert.Equal(FilterOperate.Less.ToOperateCode(), "less");
            Assert.Equal(FilterOperate.LessOrEqual.ToOperateCode(), "lessorequal");
            Assert.Equal(FilterOperate.Greater.ToOperateCode(), "greater");
            Assert.Equal(FilterOperate.GreaterOrEqual.ToOperateCode(), "greaterorequal");
            Assert.Equal(FilterOperate.StartsWith.ToOperateCode(), "startswith");
            Assert.Equal(FilterOperate.EndsWith.ToOperateCode(), "endswith");
            Assert.Equal(FilterOperate.Contains.ToOperateCode(), "contains");
        }

        [Fact()]
        public void GetFilterOperateTest()
        {
            Assert.Throws<ArgumentNullException>(() => FilterHelper.GetFilterOperate(null));
            Assert.Throws<ArgumentException>(() => FilterHelper.GetFilterOperate(""));

            Assert.Equal(FilterHelper.GetFilterOperate("and"), FilterOperate.And);
            Assert.Equal(FilterHelper.GetFilterOperate("or"), FilterOperate.Or);
            Assert.Equal(FilterHelper.GetFilterOperate("equal"), FilterOperate.Equal);
            Assert.Equal(FilterHelper.GetFilterOperate("notequal"), FilterOperate.NotEqual);
            Assert.Equal(FilterHelper.GetFilterOperate("less"), FilterOperate.Less);
            Assert.Equal(FilterHelper.GetFilterOperate("lessorequal"), FilterOperate.LessOrEqual);
            Assert.Equal(FilterHelper.GetFilterOperate("greater"), FilterOperate.Greater);
            Assert.Equal(FilterHelper.GetFilterOperate("greaterorequal"), FilterOperate.GreaterOrEqual);
            Assert.Equal(FilterHelper.GetFilterOperate("startswith"), FilterOperate.StartsWith);
            Assert.Equal(FilterHelper.GetFilterOperate("endswith"), FilterOperate.EndsWith);
            Assert.Equal(FilterHelper.GetFilterOperate("contains"), FilterOperate.Contains);
        }

    }
}