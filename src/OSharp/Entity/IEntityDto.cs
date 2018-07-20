// -----------------------------------------------------------------------
//  <copyright file="IEntityDto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-19 3:34</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Entity
{
    /// <summary>
    /// 定义输入DTO
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IInputDto<TKey>
    {
        /// <summary>
        /// 获取或设置 主键，唯一标识
        /// </summary>
        TKey Id { get; set; }
    }


    /// <summary>
    /// 定义输出DTO
    /// </summary>
    public interface IOutputDto
    { }


    /// <summary>
    /// 定义数据权限的更新，删除状态
    /// </summary>
    public interface IDataAuthEnabled
    {
        /// <summary>
        /// 获取或设置 是否可更新的数据权限状态
        /// </summary>
        bool Updatable { get; set; }

        /// <summary>
        /// 获取或设置 是否可删除的数据权限状态
        /// </summary>
        bool Deletable { get; set; }
    }
}