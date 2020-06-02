// -----------------------------------------------------------------------
// <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//     可以在此类进行继承重写来扩展基类 MessageControllerBase
// </once-generated>
//
//  <copyright file="Message.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>https://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
// -----------------------------------------------------------------------

using OSharp.Filter;

using Liuliu.Demo.Infos;
using Liuliu.Demo.Infos.Dtos;

using Microsoft.AspNetCore.Mvc;


namespace Liuliu.Demo.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 管理控制器: 站内信信息
    /// </summary>
    public class MessageController : MessageControllerBase
    {
        /// <summary>
        /// 初始化一个<see cref="MessageController"/>类型的新实例
        /// </summary>
        public MessageController(IInfosContract infosContract,
            IFilterService filterService)
            : base(infosContract, filterService)
        { }

        /// <summary>
        /// 读取站内信列表信息
        /// </summary>
        /// <param name="request">页请求信息</param>
        /// <returns>站内信列表分页信息</returns>
        public override PageData<MessageOutputDto> Read(PageRequest request)
        {
            return base.Read(request);
        }
    }
}
