// -----------------------------------------------------------------------
//  <copyright file="SettingsController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:50</last-date>
// -----------------------------------------------------------------------

using OSharp.Hosting.Systems.Dtos;


namespace OSharp.Hosting.Apis.Areas.Admin.Controllers;

[ModuleInfo(Order = 1, Position = "Systems", PositionName = "系统管理模块")]
[Description("管理-系统设置")]
public class SettingsController : AdminApiControllerBase
{
    private readonly IServiceProvider _provider;

    /// <summary>
    /// 初始化一个<see cref="SettingsController"/>类型的新实例
    /// </summary>
    public SettingsController(IServiceProvider provider)
        : base(provider)
    {
        _provider = provider;
    }

    private IKeyValueStore KeyValueStore => _provider.GetRequiredService<IKeyValueStore>();

    /// <summary>
    /// 读取设置
    /// </summary>
    /// <param name="root">设置根节点，如投票设置为Votes</param>
    /// <returns>相应节点的设置信息</returns>
    [HttpGet]
    [ModuleInfo]
    [Description("读取设置")]
    public SettingOutputDto Read(string root)
    {
        ISetting setting;
        switch (root)
        {
            case "System":
                setting = KeyValueStore.GetSetting<SystemSetting>();
                break;
            default:
                throw new OsharpException($"未知的设置根节点: {root}");
        }

        return new SettingOutputDto(setting);
    }

    /// <summary>
    /// 保存指定设置项
    /// </summary>
    /// <param name="dto">设置信息</param>
    /// <returns>JSON操作结果</returns>
    [HttpPost]
    [ModuleInfo]
    [Description("保存设置")]
    [UnitOfWork]
    public async Task<AjaxResult> Update(SettingInputDto dto)
    {
        Check.NotNull(dto, nameof(dto));

        Type type = Type.GetType(dto.SettingTypeName);
        if (type == null)
        {
            return new AjaxResult($"设置类型\"{dto.SettingTypeName}\"无法找到");
        }
        ISetting setting = JsonConvert.DeserializeObject(dto.SettingJson, type) as ISetting;
        OperationResult result = await KeyValueStore.SaveSetting(setting);
        if (result.Succeeded)
        {
            return new AjaxResult("设置保存成功");
        }
        return result.ToAjaxResult();
    }
}
