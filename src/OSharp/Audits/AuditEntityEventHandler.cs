// -----------------------------------------------------------------------
//  <copyright file="AuditEntityStoreEventHandler.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-01 21:39</last-date>
// -----------------------------------------------------------------------

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Dependency;
using OSharp.EventBuses;
using OSharp.Extensions;


namespace OSharp.Audits
{
    /// <summary>
    /// 数据审计信息处理器
    /// </summary>
    [Dependency(ServiceLifetime.Transient, AddSelf = true)]
    public class AuditEntityEventHandler : EventHandlerBase<AuditEntityEventData>
    {
        private readonly ScopedDictionary _scopedDictionary;

        /// <summary>
        /// 初始化一个<see cref="AuditEntityEventHandler"/>类型的新实例
        /// </summary>
        public AuditEntityEventHandler(ScopedDictionary scopedDictionary)
        {
            _scopedDictionary = scopedDictionary;
        }

        /// <summary>
        /// 事件处理
        /// </summary>
        /// <param name="eventData">事件源数据</param>
        public override void Handle(AuditEntityEventData eventData)
        {
            eventData.CheckNotNull("eventData");

            AuditOperationEntry operation = _scopedDictionary.AuditOperation;
            if (operation == null)
            {
                return;
            }
            foreach (AuditEntityEntry auditEntity in eventData.AuditEntities)
            {
                SetAddedId(auditEntity);
                operation.EntityEntries.Add(auditEntity);
            }
        }

        /// <summary>
        /// 异步事件处理
        /// </summary>
        /// <param name="eventData">事件源数据</param>
        /// <param name="cancelToken">异步取消标识</param>
        /// <returns>是否成功</returns>
        public override Task HandleAsync(AuditEntityEventData eventData, CancellationToken cancelToken = default(CancellationToken))
        {
            eventData.CheckNotNull("eventData");
            cancelToken.ThrowIfCancellationRequested();

            AuditOperationEntry operation = _scopedDictionary.AuditOperation;
            if (operation == null)
            {
                return Task.FromResult(0);
            }
            foreach (AuditEntityEntry auditEntity in eventData.AuditEntities)
            {
                SetAddedId(auditEntity);
                operation.EntityEntries.Add(auditEntity);
            }
            return Task.FromResult(0);
        }

        private static void SetAddedId(AuditEntityEntry entry)
        {
            if (entry.OperateType == OperateType.Insert)
            {
                dynamic entity = entry.Entity;
                entry.EntityKey = entity.Id.ToString();
                AuditPropertyEntry property = entry.PropertyEntries.FirstOrDefault(m => m.FieldName == "Id");
                if (property != null)
                {
                    property.NewValue = entity.Id.ToString();
                }
            }
        }
    }
}