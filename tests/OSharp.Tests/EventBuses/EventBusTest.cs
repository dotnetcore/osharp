using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;

using OSharp.EventBuses;

using Shouldly;

using Xunit;

namespace OSharp.Tests.EventBuses
{
    public class EventBusTests
    {
        [Fact]
        public void Default_Singleton_Test()
        {
            EventBus bus1 = EventBus.Default;
            EventBus bus2 = EventBus.Default;
            bus1.ShouldBe(bus2);
        }

        [Fact]
        public void Default_Ioc_Singleton_Test()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddOSharp();
            IServiceProvider provider = services.BuildServiceProvider();

            EventBus defaultBus = EventBus.Default;
            EventBus iocBus = provider.GetService<EventBus>();
            defaultBus.ShouldBe(iocBus);
        }
    }
}
