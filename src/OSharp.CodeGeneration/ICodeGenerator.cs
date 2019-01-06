// -----------------------------------------------------------------------
//  <copyright file="ICodeGenerator.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-01-07 2:50</last-date>
// -----------------------------------------------------------------------

using OSharp.CodeGeneration.Schema;


namespace OSharp.CodeGeneration
{
    /// <summary>
    /// 定义代码生成器
    /// </summary>
    public interface ICodeGenerator
    {
        /// <summary>
        /// 生成项目文件
        /// </summary>
        /// <param name="project">项目元数据</param>
        /// <returns>项目代码</returns>
        CodeFile[] GenerateProjectCode(ProjectMetadata project);

        /// <summary>
        /// 由实体元数据生成实体类代码
        /// </summary>
        /// <param name="entity">实体元数据</param>
        /// <returns>实体类代码</returns>
        CodeFile GenerateEntityCode(EntityMetadata entity);

        /// <summary>
        /// 由实体元数据生成输入DTO类代码
        /// </summary>
        /// <param name="entity">实体元数据</param>
        /// <returns>输入DTO类代码</returns>
        CodeFile GenerateInputDtoCode(EntityMetadata entity);

        /// <summary>
        /// 由实体元数据生成输出DTO类代码
        /// </summary>
        /// <param name="entity">实体元数据</param>
        /// <returns>输出DTO类代码</returns>
        CodeFile GenerateOutputDtoCode(EntityMetadata entity);

        /// <summary>
        /// 由模块元数据生成模块业务契约接口代码
        /// </summary>
        /// <param name="module">模块元数据</param>
        /// <returns>模块业务契约接口代码</returns>
        CodeFile GenerateServiceContract(ModuleMetadata module);

        /// <summary>
        /// 由模块元数据生成模块业务综合实现类代码
        /// </summary>
        /// <param name="module">模块元数据</param>
        /// <returns>模块业务综合实现类代码</returns>
        CodeFile GenerateServiceMainImpl(ModuleMetadata module);

        /// <summary>
        /// 由模块元数据生成模块业务单实体实现类代码
        /// </summary>
        /// <param name="entity">实体元数据</param>
        /// <returns>模块业务单实体实现类代码</returns>
        CodeFile GenerateServiceEntityImpl(EntityMetadata entity);

        /// <summary>
        /// 由模块元数据生成实体数据映射配置类代码
        /// </summary>
        /// <param name="entity">实体元数据</param>
        /// <returns>实体数据映射配置类代码</returns>
        CodeFile GenerateEntityConfiguration(EntityMetadata entity);

        /// <summary>
        /// 由模块元数据生成实体管理控制器类代码
        /// </summary>
        /// <param name="entity">实体元数据</param>
        /// <returns>实体管理控制器类代码</returns>
        CodeFile GenerateAdminController(EntityMetadata entity);
    }
}