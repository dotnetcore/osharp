using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using OSharp.EventBuses;
using OSharp.Maths;

using Shouldly;

using Xunit;

namespace OSharp.Tests.EventBuses
{
    public class EventBusTests
    {
        public EventBusTests()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddOSharp();
            services.AddLogging();
            IServiceProvider provider = services.BuildServiceProvider();
            ServiceLocator.Instance.TrySetApplicationServiceProvider(provider);
        }
        
        [Fact]
        public void Register_Test()
        {
            EventBus.Default.UnregisterAll<HelloEventData>();

            EventBus.Default.Register<HelloEventData, HelloEventHandler>();
            HelloEventData data = new HelloEventData("hello world");
            EventBus.Default.Trigger(data);
            Thread.Sleep(50);
            data.List.ShouldContain(data.Message);
            data.List.Clear();
            EventBus.Default.UnregisterAll<HelloEventData>();

            Action<HelloEventData> action = m => m.List.Add(m.Message);
            EventBus.Default.Register<HelloEventData>(action);
            EventBus.Default.Trigger(data);
            Thread.Sleep(50);
            data.List.ShouldContain(data.Message);
            data.List.Clear();
            EventBus.Default.Unregister(action);

            IEventHandler<HelloEventData> handler = new HelloEventHandler();
            EventBus.Default.Register<HelloEventData>(handler);
            EventBus.Default.Trigger(typeof(HelloEventData), (IEventData)data);
            Thread.Sleep(50);
            data.List.ShouldContain(data.Message);
            data.List.Clear();
            EventBus.Default.Unregister(handler);
        }

        [Fact]
        public async Task TriggerAsync_Test()
        {
            EventBus.Default.UnregisterAll<HelloEventData>();

            EventBus.Default.Register<HelloEventData, HelloEventHandler>();
            HelloEventData data = new HelloEventData("hello world");
            await EventBus.Default.TriggerAsync(data);
            Thread.Sleep(50);
            data.List.ShouldContain(data.Message);
            data.List.Clear();
            EventBus.Default.UnregisterAll<HelloEventData>();
        }

        private class HelloEventData : EventData
        {
            /// <summary>
            /// 初始化一个<see cref="HelloEventData"/>类型的新实例
            /// </summary>
            public HelloEventData(string message)
            {
                List = new List<string>();
                Message = message;
            }

            public string Message { get; }

            public ICollection<string> List { get; }
        }


        private class HelloEventHandler : IEventHandler<HelloEventData>
        {
            /// <summary>
            /// 事件处理
            /// </summary>
            /// <param name="eventData">事件源数据</param>
            public void HandleEvent(HelloEventData eventData)
            {
                eventData.List.Add(eventData.Message);
            }
        }
    }
}
