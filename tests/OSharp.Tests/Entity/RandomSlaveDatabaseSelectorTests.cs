using Xunit;
using OSharp.Entity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using NSubstitute;

using OSharp.Core.Options;


namespace OSharp.Entity.Tests
{
    public class RandomSlaveDatabaseSelectorTests
    {
        [Fact()]
        public void SelectTest()
        {
            var logger = Substitute.For<ILogger<RandomSlaveDatabaseSelector>>();
            var factory = Substitute.For<ILoggerFactory>();
            var provider = Substitute.For<IServiceProvider>();
            provider.GetService<ILoggerFactory>().Returns(factory);
            factory.CreateLogger<RandomSlaveDatabaseSelector>().Returns(logger);

            RandomSlaveDatabaseSelector selector = new(provider);
            SlaveDatabaseOptions[] slaves =
            {
                new() { ConnectionString = "Conn01", Name = "SqlServer", Weight = 1 },
                new() { ConnectionString = "Conn02", Name = "MySql", Weight = 2 },
                new() { ConnectionString = "Conn03", Name = "Sqlite", Weight = 3 }
            };
            SlaveDatabaseOptions slave = selector.Select(slaves);
            Assert.NotNull(slave);
        }
    }
}