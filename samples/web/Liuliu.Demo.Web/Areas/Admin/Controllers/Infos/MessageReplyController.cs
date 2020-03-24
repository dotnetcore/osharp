// -----------------------------------------------------------------------
// <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//     可以在此类进行继承重写来扩展基类 MessageReplyControllerBase
// </once-generated>
//
//  <copyright file="MessageReply.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>https://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
// -----------------------------------------------------------------------

using OSharp.Filter;

using Liuliu.Demo.Infos;


namespace Liuliu.Demo.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 管理控制器: 站内信回复信息
    /// </summary>
    public class MessageReplyController : MessageReplyControllerBase
    {
        /// <summary>
        /// 初始化一个<see cref="MessageReplyController"/>类型的新实例
        /// </summary>
        public MessageReplyController(IInfosContract infosContract,
            IFilterService filterService)
            : base(infosContract, filterService)
        { }
    }
}
