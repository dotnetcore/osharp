// -----------------------------------------------------------------------
//  <copyright file="CodeSetting.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-08 22:30</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

using Newtonsoft.Json;

using OSharp.CodeGeneration.Generates;
using OSharp.CodeGeneration.Templates;
using OSharp.Entity;
using OSharp.Extensions;


namespace OSharp.CodeGeneration.Services.Entities
{
    /// <summary>
    /// 实体类：代码模板
    /// </summary>
    [Description("代码模板")]
    [TableNamePrefix("CodeGen")]
    [DebuggerDisplay("{Name} - {MetadataType} - {TemplateFile}")]
    public class CodeTemplate : EntityBase<Guid>, ILockable, ICreatedTime
    {
        /// <summary>
        /// 获取或设置 配置名称
        /// </summary>
        [Required, StringLength(200)]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 元数据类型
        /// </summary>
        public MetadataType MetadataType { get; set; }

        /// <summary>
        /// 获取或设置 模板文件，默认内置，也可以由用户自定义加载
        /// </summary>
        [Required, StringLength(500)]
        public string TemplateFile { get; set; }

        /// <summary>
        /// 获取或设置 排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 获取或设置 代码输出文件名格式
        /// </summary>
        [Required, StringLength(300)]
        public string OutputFileFormat { get; set; }

        /// <summary>
        /// 获取或设置 是否只生成一次
        /// </summary>
        public bool IsOnce { get; set; }

        /// <summary>
        /// 获取或设置 系统类型
        /// </summary>
        public bool IsSystem { get; set; }

        /// <summary>获取或设置 是否锁定当前信息</summary>
        public bool IsLocked { get; set; }

        /// <summary>获取或设置 创建时间</summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 获取或设置 项目模板集合
        /// </summary>
        [JsonIgnore]
        public virtual ICollection<CodeProjectTemplate> ProjectTemplates { get; set; } = new List<CodeProjectTemplate>();

        public Type GetInnerTemplateType()
        {
            switch (Name)
            {
                case "cs_实体类":
                    return typeof(cs_Entity);
                case "cs_输入DTO类":
                    return typeof(cs_InputDto);
                case "cs_输出DTO类":
                    return typeof(cs_OutputDto);
                case "cs_模块Pack类":
                    return typeof(cs_ServicePack);
                case "cs_模块服务契约接口":
                    return typeof(cs_ServiceContract);
                case "cs_模块服务综合实现基类":
                    return typeof(cs_ServiceMainImplBase);
                case "cs_模块服务综合实现类":
                    return typeof(cs_ServiceMainImpl);
                case "cs_模块服务实体实现基类":
                    return typeof(cs_ServiceEntityImplBase);
                case "cs_实体数据映射配置类":
                    return typeof(cs_EntityConfiguration);
                case "cs_实体管理控制器基类":
                    return typeof(cs_AdminControllerBase);
                case "cs_实体管理控制器类":
                    return typeof(cs_AdminController);
                case "ng_Alain模块":
                    return typeof(ng_AlainModule);
                case "ng_Alain模块路由":
                    return typeof(ng_AlainRouting);
                case "ng_Alain模块组件":
                    return typeof(ng_AlainComponent);
                case "ng_Alain模块组件Html":
                    return typeof(ng_AlainComponentHtml);
                case "ng_Alain其他数据":
                    return typeof(ng_AlainOther);
                case "vue_Vben国际化多语言英文":
                    return typeof(vue_VbenLocalesLangEn);
                case "vue_Vben国际化多语言简体中文":
                    return typeof(vue_VbenLocalesLangZhCN);
                case "vue_Vben路由器路由":
                    return typeof(vue_VbenRouterRoutes);
                case "vue_Vben存储Store":
                    return typeof(vue_VbenStore);
                case "vue_Vben视图Vue":
                    return typeof(vue_VbenViewsVue);
                default:
                    return null;
            }
        }

        /// <summary>
        /// 获取项目代码输出文件名
        /// </summary>
        public string GetCodeFileName(CodeProject project)
        {
            string fileName = OutputFileFormat.Replace("{Project.NamespacePrefix}", project.NamespacePrefix)
                .Replace("{Project.Name:Lower}", project.Name.ToLowerCase())
                .Replace("{Project.Name}", project.Name);
            return fileName;
        }

        /// <summary>
        /// 获取项目代码输出文件名
        /// </summary>
        public string GetCodeFileName(CodeModule module)
        {
            string fileName = OutputFileFormat.Replace("{Project.NamespacePrefix}", module.Project.NamespacePrefix)
                .Replace("{Module.Name:Lower}", module.Name.ToLowerCase())
                .Replace("{Module.Name}", module.Name);
            return fileName;
        }

        /// <summary>
        /// 获取项目代码输出文件名
        /// </summary>
        public string GetCodeFileName(CodeEntity entity)
        {
            string fileName = OutputFileFormat.Replace("{Project.NamespacePrefix}", entity.Module.Project.NamespacePrefix)
                .Replace("{Module.Name:Lower}", entity.Module.Name.ToLowerCase())
                .Replace("{Entity.Name:Lower}", entity.Name.ToLowerCase())
                .Replace("{Module.Name}", entity.Module.Name).Replace("{Entity.Name}", entity.Name);
            return fileName;
        }

    }
}
