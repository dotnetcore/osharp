using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;

using OSharp.EventBuses;
using OSharp.Maths;

using Shouldly;

using Xunit;

namespace OSharp.Tests.EventBuses
{
    public class EventBusTests
    {
        [Fact]
        public void Default_Ioc_Singleton_Test()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddOSharp();
            services.AddLogging();
            IServiceProvider provider = services.BuildServiceProvider();
            ServiceLocator.Instance.TrySetApplicationServiceProvider(provider);
            EventBus defaultBus = EventBus.Default;
            EventBus iocBus = provider.GetService<EventBus>();
            defaultBus.ShouldBe(iocBus);
        }
        
    }
}
