// -----------------------------------------------------------------------
//  <copyright file="SequenceSlaveDatabaseSelectorTests.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-03-21 12:17</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using NSubstitute;

using OSharp.Core.Options;

using Xunit;


namespace OSharp.Entity.Tests
{
    public class SequenceSlaveDatabaseSelectorTests
    {
        [Fact()]
        public void SelectTest()
        {
            var logger = Substitute.For<ILogger<SequenceSlaveDatabaseSelector>>();
            var factory = Substitute.For<ILoggerFactory>();
            var provider = Substitute.For<IServiceProvider>();
            provider.GetService<ILoggerFactory>().Returns(factory);
            factory.CreateLogger<SequenceSlaveDatabaseSelector>().Returns(logger);

            SequenceSlaveDatabaseSelector selector = new(provider);
            SlaveDatabaseOptions[] slaves =
            {
                new() { ConnectionString = "Conn01", Name = "SqlServer", Weight = 1 },
                new() { ConnectionString = "Conn02", Name = "MySql", Weight = 2 },
                new() { ConnectionString = "Conn03", Name = "Sqlite", Weight = 3 }
            };
            SlaveDatabaseOptions slave = selector.Select(slaves);
            Assert.Equal("SqlServer", slave.Name);
            slave = selector.Select(slaves);
            Assert.Equal("MySql", slave.Name);
            slave = selector.Select(slaves);
            Assert.Equal("Sqlite", slave.Name);
            slave = selector.Select(slaves);
            Assert.Equal("SqlServer", slave.Name);
        }
    }
}