// -----------------------------------------------------------------------
//  <copyright file="CodeType.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-01-06 22:21</last-date>
// -----------------------------------------------------------------------

namespace OSharp.CodeGeneration.Schema
{
    /// <summary>
    /// 表示代码类型的枚举
    /// </summary>
    public enum CodeType
    {
        /// <summary>
        /// 实体类
        /// </summary>
        Entity,
        /// <summary>
        /// 输入DTO类
        /// </summary>
        InputDto,
        /// <summary>
        /// 输出DTO类
        /// </summary>
        OutputDto,
        /// <summary>
        /// 服务接口
        /// </summary>
        ServiceContract,
        /// <summary>
        /// 服务综合实现
        /// </summary>
        ServiceMainImpl,
        /// <summary>
        /// 服务实体实现
        /// </summary>
        ServiceEntityImpl,
        /// <summary>
        /// 实体数据映射类
        /// </summary>
        EntityConfiguration,
        /// <summary>
        /// 实体控制器类
        /// </summary>
        AdminController
    }
}