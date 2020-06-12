// -----------------------------------------------------------------------
//  <copyright file="MessageType.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-09-10 18:39</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Hosting.Infos.Entities
{
    /// <summary>
    /// 表示消息类型的枚举
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        ///   系统消息，用于系统对全体人员的消息类型表示
        /// </summary>
        System = 0,

        /// <summary>
        ///   公共消息，用于系统对局部范围的人员的消息类型表示
        /// </summary>
        Public = 1,

        /// <summary>
        ///   私人消息，用于个人对个人的消息类型表示
        /// </summary>
        Private = 2
    }
}