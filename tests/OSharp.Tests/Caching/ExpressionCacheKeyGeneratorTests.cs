// -----------------------------------------------------------------------
//  <copyright file="ExpressionCacheKeyGeneratorTests.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-12-31 1:42</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq.Expressions;

using OSharp.UnitTest.Infrastructure;

using Shouldly;

using Xunit;


namespace OSharp.Caching.Tests
{
    public class ExpressionCacheKeyGeneratorTests
    {
        [Fact]
        public void GetKeyTest()
        {
            Expression<Func<TestEntity, bool>> exp = m => m.Name.StartsWith("001") && m.IsDeleted;
            ExpressionCacheKeyGenerator generator = new ExpressionCacheKeyGenerator(exp);
            string key = generator.GetKey();
            key.ShouldBe("m => (m.Name.StartsWith(\"001\") AndAlso m.IsDeleted)");

            key = generator.GetKey("001", true);
            key.ShouldBe("m => (m.Name.StartsWith(\"001\") AndAlso m.IsDeleted)001,True");
        }
    }
}