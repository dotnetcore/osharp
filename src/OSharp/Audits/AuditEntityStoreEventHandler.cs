using System;
using System.Collections.Generic;
using System.Text;

using OSharp.EventBuses;


namespace OSharp.Audits
{
    /// <summary>
    /// 数据审计存储处理器
    /// </summary>
    public class AuditEntityStoreEventHandler : IEventHandler<AuditEntityEventData>
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
        public void HandleEvent(AuditEntityEventData eventData)
        {
            _auditStore.SetAuditDatas(eventData.AuditEntities);
        }
    }
}
