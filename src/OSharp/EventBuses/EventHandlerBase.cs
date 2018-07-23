using System;
using System.Threading;
using System.Threading.Tasks;


namespace OSharp.EventBuses
{
    /// <summary>
    /// 事件处理器基类
    /// </summary>
    public abstract class EventHandlerBase<TEventData> : IEventHandler<TEventData> where TEventData : IEventData
    {
        /// <summary>
        /// 是否可处理指定事件
        /// </summary>
        /// <param name="eventData">事件源数据</param>
        /// <returns>是否可处理</returns>
        public virtual bool CanHandle(IEventData eventData)
        {
            return eventData.GetType() == typeof(TEventData);
        }

        /// <summary>
        /// 事件处理
        /// </summary>
        /// <param name="eventData">事件源数据</param>
        public virtual void Handle(IEventData eventData)
        {
            if (!CanHandle(eventData))
            {
                return;
            }
            Handle((TEventData)eventData);
        }

        /// <summary>
        /// 异步事件处理
        /// </summary>
        /// <param name="eventData">事件源数据</param>
        /// <param name="cancelToken">异步取消标识</param>
        /// <returns></returns>
        public virtual Task HandleAsync(IEventData eventData, CancellationToken cancelToken = default(CancellationToken))
        {
            if (!CanHandle(eventData))
            {
                return Task.FromResult(0);
            }
            return HandleAsync((TEventData)eventData, cancelToken);
        }

        /// <summary>
        /// 事件处理
        /// </summary>
        /// <param name="eventData">事件源数据</param>
        public abstract void Handle(TEventData eventData);

        /// <summary>
        /// 异步事件处理
        /// </summary>
        /// <param name="eventData">事件源数据</param>
        /// <param name="cancelToken">异步取消标识</param>
        /// <returns>是否成功</returns>
        public virtual Task HandleAsync(TEventData eventData, CancellationToken cancelToken = default(CancellationToken))
        {
            throw new NotSupportedException("当前事件处理器不支持异步事件处理");
        }
    }
}
