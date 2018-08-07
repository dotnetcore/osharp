// -----------------------------------------------------------------------
//  <copyright file="SettingsController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:50</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using OSharp.CodeGenerator;
using OSharp.Core.Modules;


namespace Liuliu.Demo.Web.Areas.Admin.Controllers
{
    [ModuleInfo(Order = 1, Position = "System", PositionName = "系统管理模块")]
    [Description("管理-系统设置")]
    public class SettingsController : AdminApiController
    {
        private readonly ITypeMetadataHandler _metadataHandler;

        public SettingsController(ITypeMetadataHandler metadataHandler)
        {
            _metadataHandler = metadataHandler;
        }

        /// <summary>
        /// 获取指定类型类别的元数据
        /// </summary>
        /// <param name="type">类别</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [AllowAnonymous]
        [Description("获取类型元数据")]
        public TypeMetadata[] GetTypeMetadata(string type)
        {
            switch (type?.ToLower())
            {
                case "entity":
                    return _metadataHandler.GetEntityTypeMetadatas();
                case "inputdto":
                    return _metadataHandler.GetInputDtoMetadatas();
                case "outputdto":
                    return _metadataHandler.GetOutputDtoMetadata();
            }
            return new TypeMetadata[0];
        }
    }
}