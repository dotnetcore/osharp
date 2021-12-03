// -----------------------------------------------------------------------
//  <copyright file="WeightSlaveDatabaseSelectorTests.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-03-21 12:17</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using NSubstitute;

using OSharp.Core.Options;

using Xunit;


namespace OSharp.Entity.Tests
{
    public class WeightSlaveDatabaseSelectorTests
    {
        [Fact()]
        public void SelectTest()
        {
            var logger = Substitute.For<ILogger<WeightSlaveDatabaseSelector>>();
            var factory = Substitute.For<ILoggerFactory>();
            var provider = Substitute.For<IServiceProvider>();
            provider.GetService<ILoggerFactory>().Returns(factory);
            factory.CreateLogger<WeightSlaveDatabaseSelector>().Returns(logger);

            WeightSlaveDatabaseSelector selector = new(provider);
            SlaveDatabaseOptions[] slaves =
            {
                new() { ConnectionString = "Conn01", Name = "SqlServer", Weight = 6 },
                new() { ConnectionString = "Conn02", Name = "Mysql", Weight = 2 },
                new() { ConnectionString = "Conn03", Name = "Sqlite", Weight = 2 }
            };
            var list = new List<SlaveDatabaseOptions>();
            for (int i = 0; i < slaves.Sum(m => m.Weight); i++)
            {
                list.Add(selector.Select(slaves));
            }
            Assert.Equal(2, list.Count(m => m.Name == "Sqlite"));
            Assert.Equal(2, list.Count(m => m.Name == "Mysql"));
            Assert.Equal(6, list.Count(m => m.Name == "SqlServer"));
        }
    }
}