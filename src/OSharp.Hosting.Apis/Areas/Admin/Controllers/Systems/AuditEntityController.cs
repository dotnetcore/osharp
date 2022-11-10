// -----------------------------------------------------------------------
//  <copyright file="AuditEntityController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-02 14:57</last-date>
// -----------------------------------------------------------------------

using OSharp.Hosting.Systems;
using OSharp.Hosting.Systems.Dtos;
using OSharp.Hosting.Systems.Entities;


namespace OSharp.Hosting.Apis.Areas.Admin.Controllers;

[ModuleInfo(Order = 3, Position = "Systems", PositionName = "系统管理模块")]
[Description("管理-数据审计信息")]
public class AuditEntityController : AdminApiControllerBase
{
    private readonly IServiceProvider _provider;
        
    /// <summary>
    /// 初始化一个<see cref="AuditEntityController"/>类型的新实例
    /// </summary>
    public AuditEntityController(IServiceProvider provider) : base(provider)
    {
        _provider = provider;
    }

    private IAuditContract AuditContract => _provider.GetRequiredService<IAuditContract>();

    /// <summary>
    /// 读取数据审计信息列表
    /// </summary>
    /// <param name="request">页请求</param>
    /// <returns></returns>
    [HttpPost]
    [ModuleInfo]
    [Description("读取")]
    public AjaxResult Read(PageRequest request)
    {
        Expression<Func<AuditEntity, bool>> predicate = FilterService.GetExpression<AuditEntity>(request.FilterGroup);
        PageResult<AuditEntityOutputDto> page;
        //有操作参数，是从操作列表来的
        if (request.FilterGroup.Rules.Any(m => m.Field == "OperationId"))
        {
            page = AuditContract.AuditEntities.ToPage(predicate, request.PageCondition, m => new AuditEntityOutputDto
            {
                Id = m.Id,
                Name = m.Name,
                TypeName = m.TypeName,
                EntityKey = m.EntityKey,
                OperateType = m.OperateType,
                Properties = AuditContract.AuditProperties.Where(n => n.AuditEntityId == m.Id).OrderBy(n => n.FieldName != "Id").ThenBy(n => n.FieldName).Select(n => new AuditPropertyOutputDto()
                {
                    DisplayName = n.DisplayName,
                    FieldName = n.FieldName,
                    OriginalValue = n.OriginalValue,
                    NewValue = n.NewValue,
                    DataType = n.DataType
                }).ToList()
            });
            return new AjaxResult(page);
        }
        request.AddDefaultSortCondition(new SortCondition("Operation.CreatedTime", ListSortDirection.Descending));
        page = AuditContract.AuditEntities.ToPage(predicate, request.PageCondition, m => new AuditEntityOutputDto
        {
            Id = m.Id,
            Name = m.Name,
            TypeName = m.TypeName,
            EntityKey = m.EntityKey,
            OperateType = m.OperateType,
            NickName = m.Operation.NickName,
            FunctionName = m.Operation.FunctionName,
            CreatedTime = m.Operation.CreatedTime,
            Properties = AuditContract.AuditProperties.Where(n => n.AuditEntityId == m.Id).OrderBy(n => n.FieldName != "Id").ThenBy(n => n.FieldName).Select(n => new AuditPropertyOutputDto()
            {
                DisplayName = n.DisplayName,
                FieldName = n.FieldName,
                OriginalValue = n.OriginalValue,
                NewValue = n.NewValue,
                DataType = n.DataType
            }).ToList()
        });
        return new AjaxResult(page.ToPageData());
    }
}
