// -----------------------------------------------------------------------
//  <copyright file="EntityBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-18 11:27</last-date>
// -----------------------------------------------------------------------

using System;

using OSharp.Data;


namespace OSharp.Entity
{
    /// <summary>
    /// 实体类基类
    /// </summary>
    public abstract class EntityBase<TKey> : IEntity<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// 初始化一个<see cref="EntityBase{TKey}"/>类型的新实例
        /// </summary>
        protected EntityBase()
        {
            if (typeof(TKey) == typeof(Guid))
            {
                Id = CombHelper.NewComb().CastTo<TKey>();
            }
        }

        /// <summary>
        /// 获取或设置 编号
        /// </summary>
        public TKey Id { get; set; }

        /// <summary>
        /// 判断两个实体是否是同一数据记录的实体
        /// </summary>
        /// <param name="obj">要比较的实体信息</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            EntityBase<TKey> entity = obj as EntityBase<TKey>;
            if (entity == null)
            {
                return false;
            }
            return entity.Id.Equals(Id);
        }

        /// <summary>
        /// 用作特定类型的哈希函数。
        /// </summary>
        /// <returns>
        /// 当前 <see cref="T:System.Object"/> 的哈希代码。<br/>
        /// 如果<c>Id</c>为<c>null</c>则返回0，
        /// 如果不为<c>null</c>则返回<c>Id</c>对应的哈希值
        /// </returns>
        public override int GetHashCode()
        {
            if (Id == null)
            {
                return 0;
            }
            return Id.ToString().GetHashCode();
        }
    }
}