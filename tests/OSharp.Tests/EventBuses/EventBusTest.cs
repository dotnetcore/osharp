using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Dependency;
using OSharp.EventBuses;
using OSharp.UnitTest.Infrastructure;

using Shouldly;

using Xunit;

namespace OSharp.Tests.IEventBuses
{
    public class IEventBusTests : AppUnitTestBase
    {
        [Fact]
        public void Subscribe_Test()
        {
            IEventBus bus = ServiceLocator.Instance.GetService<IEventBus>();

            bus.UnsubscribeAll<HelloEventData>();

            bus.Subscribe<HelloEventData, HelloEventHandler>();
            HelloEventData data = new HelloEventData("hello world");
            bus.Publish(data);
            //Thread.Sleep(50);
            data.List.ShouldContain(data.Message);
            data.List.Clear();
            bus.UnsubscribeAll<HelloEventData>();

            Action<HelloEventData> action = m => m.List.Add(m.Message);
            bus.Subscribe<HelloEventData>(action);
            bus.Publish(data);
            //Thread.Sleep(50);
            data.List.ShouldContain(data.Message);
            data.List.Clear();
            bus.Unsubscribe(action);

            IEventHandler<HelloEventData> handler = new HelloEventHandler();
            bus.Subscribe<HelloEventData>(handler);
            bus.Publish(typeof(HelloEventData), (IEventData)data);
            //Thread.Sleep(50);
            data.List.ShouldContain(data.Message);
            data.List.Clear();
            bus.Unsubscribe(handler);
        }

        [Fact]
        public async Task PublishAsync_Test()
        {
            IEventBus bus = ServiceLocator.Instance.GetService<IEventBus>();
            bus.UnsubscribeAll<HelloEventData>();

            bus.Subscribe<HelloEventData, HelloEventHandler>();
            HelloEventData data = new HelloEventData("hello world");
            await bus.PublishAsync(data);
            Thread.Sleep(100);
            data.List.ShouldContain(data.Message);
            data.List.Clear();
            bus.UnsubscribeAll<HelloEventData>();
        }

        private class HelloEventData : EventDataBase
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


        private class HelloEventHandler : EventHandlerBase<HelloEventData>
        {
            /// <summary>
            /// 事件处理
            /// </summary>
            /// <param name="eventData">事件源数据</param>
            public override void Handle(HelloEventData eventData)
            {
                eventData.List.Add(eventData.Message);
            }

            /// <summary>
            /// 异步事件处理
            /// </summary>
            /// <param name="eventData">事件源数据</param>
            /// <param name="cancelToken">异步取消标识</param>
            /// <returns>是否成功</returns>
            public override Task HandleAsync(HelloEventData eventData, CancellationToken cancelToken = default(CancellationToken))
            {
                return Task.Run(() => Handle(eventData), cancelToken);
            }
        }


        public class EventBusStartup : TestStartup
        {
            public override void ConfigureServices(IServiceCollection services)
            {
                services.AddHttpContextAccessor().AddLogging();

                services.AddOSharp();
            }
        }

    }

}
