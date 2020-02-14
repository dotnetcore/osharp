// -----------------------------------------------------------------------
//  <copyright file="ModuleInfoAttribute.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-10 20:13</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.Authorization.Modules
{
    /// <summary>
    /// 描述把当前功能(Controller或者Action)封装为一个模块(Module)节点，可以设置模块依赖的其他功能，模块的位置信息等
    /// 此特性用于系统初始化时自动提取模块树信息Module
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ModuleInfoAttribute : Attribute
    {
        /// <summary>
        /// 获取或设置 模块名称，为空则取功能名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 模块代码，为空则取功能Action名
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 获取或设置 层次序号
        /// </summary>
        public double Order { get; set; }

        /// <summary>
        /// 获取或设置 模块位置，父级模块，模块在树节点的位置，默认取所在类的位置，需要在命名空间与当前类之间加模块，才设置此值
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// 获取或设置 父级位置模块名称，需要在命名空间与当前类之间加模块，才设置此值
        /// </summary>
        public string PositionName { get; set; }
    }
}