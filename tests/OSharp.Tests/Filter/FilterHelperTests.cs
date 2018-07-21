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
            Assert.Equal("and", FilterOperate.And.ToOperateCode());
            Assert.Equal("or", FilterOperate.Or.ToOperateCode());
            Assert.Equal("equal", FilterOperate.Equal.ToOperateCode());
            Assert.Equal("notequal", FilterOperate.NotEqual.ToOperateCode());
            Assert.Equal("less", FilterOperate.Less.ToOperateCode());
            Assert.Equal("lessorequal", FilterOperate.LessOrEqual.ToOperateCode());
            Assert.Equal("greater", FilterOperate.Greater.ToOperateCode());
            Assert.Equal("greaterorequal", FilterOperate.GreaterOrEqual.ToOperateCode());
            Assert.Equal("startswith", FilterOperate.StartsWith.ToOperateCode());
            Assert.Equal("endswith", FilterOperate.EndsWith.ToOperateCode());
            Assert.Equal("contains", FilterOperate.Contains.ToOperateCode());
        }

        [Fact()]
        public void GetFilterOperateTest()
        {
            Assert.Throws<ArgumentNullException>(() => FilterHelper.GetFilterOperate(null));
            Assert.Throws<ArgumentException>(() => FilterHelper.GetFilterOperate(""));

            Assert.Equal(FilterOperate.And, FilterHelper.GetFilterOperate("and"));
            Assert.Equal(FilterOperate.Or, FilterHelper.GetFilterOperate("or"));
            Assert.Equal(FilterOperate.Equal, FilterHelper.GetFilterOperate("equal"));
            Assert.Equal(FilterOperate.NotEqual, FilterHelper.GetFilterOperate("notequal"));
            Assert.Equal(FilterOperate.Less, FilterHelper.GetFilterOperate("less"));
            Assert.Equal(FilterOperate.LessOrEqual, FilterHelper.GetFilterOperate("lessorequal"));
            Assert.Equal(FilterOperate.Greater, FilterHelper.GetFilterOperate("greater"));
            Assert.Equal(FilterOperate.GreaterOrEqual, FilterHelper.GetFilterOperate("greaterorequal"));
            Assert.Equal(FilterOperate.StartsWith, FilterHelper.GetFilterOperate("startswith"));
            Assert.Equal(FilterOperate.EndsWith, FilterHelper.GetFilterOperate("endswith"));
            Assert.Equal(FilterOperate.Contains, FilterHelper.GetFilterOperate("contains"));
        }

    }
}