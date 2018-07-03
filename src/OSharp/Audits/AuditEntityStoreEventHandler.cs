using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using OSharp.EventBuses;
using OSharp.Extensions;


namespace OSharp.Audits
{
    /// <summary>
    /// 数据审计存储处理器
    /// </summary>
    public class AuditEntityStoreEventHandler : EventHandlerBase<AuditEntityEventData>
    {
        private readonly IAuditStore _auditStore;

        /// <summary>
        /// 初始化一个<see cref="AuditEntityStoreEventHandler"/>类型的新实例
        /// </summary>
        public AuditEntityStoreEventHandler(IAuditStore auditStore)
        {
            _auditStore = auditStore;
        }
        
        /// <summary>
        /// 事件处理
        /// </summary>
        /// <param name="eventData">事件源数据</param>
        public override void Handle(AuditEntityEventData eventData)
        {
            eventData.CheckNotNull("eventData");
            _auditStore.SetAuditDatas(eventData.AuditEntities);
        }

        /// <summary>
        /// 异步事件处理
        /// </summary>
        /// <param name="eventData">事件源数据</param>
        /// <param name="cancelToken">异步取消标识</param>
        /// <returns>是否成功</returns>
        public override Task HandleAsync(AuditEntityEventData eventData, CancellationToken cancelToken = default(CancellationToken))
        {
            eventData.CheckNotNull("eventData" );
            cancelToken.ThrowIfCancellationRequested();
            return _auditStore.SetAuditDatasAsync(eventData.AuditEntities, cancelToken);
        }
    }
}
