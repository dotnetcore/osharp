# T4 代码生成功能说明

## 运行环境

运行T4生成多文件，需要安装如下两个VisualStudio插件
1. T4 Toolbox for Visual Studio 2017：好用的T4模板工具，Script是脚本的执行入口，Template是代码模板，生成不同类型的代码
2. Devart T4 Editor：为T4编号提供一些粗糙的智能提示和格式化，没多好用
> 要方便编写T4脚本文件，最好还是在控制台中，CS文件中写好并运行通过，再复制到T4中

## 类型元数据
由于.net standard 的运行环境与T4代码运行环境尚未完美兼容，很多时候会遇到加载底层程序集的问题，很不好用。
框架中的`OSharp.CodeGenerator`命名空间提供了获取`Entity`,`InputDto`,`OutputDto`代码元数据功能，T4脚本中使用Http的方式获取要用于生成代码的源代码元数据，T4代码只需要分析这些元数据即可较轻松的生成出目标代码。

### 元数据组成
1. 类型元数据：OSharp.CodeGenerator.TypeMetadata
    ```
    /// <summary>
    /// 类型元数据
    /// </summary>
    public class TypeMetadata
    {
        /// <summary>
        /// 初始化一个<see cref="TypeMetadata"/>类型的新实例
        /// </summary>
        public TypeMetadata()
        { }

        /// <summary>
        /// 初始化一个<see cref="TypeMetadata"/>类型的新实例
        /// </summary>
        public TypeMetadata(Type type)
        {
            if (type == null)
            {
                return;
            }
            
            Name = type.Name;
            FullName = type.FullName;
            Namespace = type.Namespace;
            Display = type.GetDescription();
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo property in properties.Where(m => !m.HasAttribute<IgnoreGenPropertyAttribute>()))
            {
                if (PropertyMetadatas == null)
                {
                    PropertyMetadatas = new List<PropertyMetadata>();
                }
                PropertyMetadatas.Add(new PropertyMetadata(property));
            }
        }
        
        /// <summary>
        /// 获取或设置 类型名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 类型全名
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 获取或设置 命名空间
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// 获取或设置 类型显示名
        /// </summary>
        public string Display { get; set; }

        /// <summary>
        /// 获取或设置 属性元数据集合
        /// </summary>
        public IList<PropertyMetadata> PropertyMetadatas { get; set; }
    }
    ```
2. 属性元数据：OSharp.CodeGenerator.PropertyMetadata
    ```
    /// <summary>
    /// 属性元数据
    /// </summary>
    public class PropertyMetadata
    {
        /// <summary>
        /// 初始化一个<see cref="PropertyMetadata"/>类型的新实例
        /// </summary>
        public PropertyMetadata()
        { }

        /// <summary>
        /// 初始化一个<see cref="PropertyMetadata"/>类型的新实例
        /// </summary>
        public PropertyMetadata(PropertyInfo property)
        {
            if (property == null)
            {
                return;
            }
            Name = property.Name;
            TypeName = property.PropertyType.FullName;
            Display = property.GetDescription();
            RequiredAttribute required = property.GetAttribute<RequiredAttribute>();
            if (required != null)
            {
                IsRequired = !required.AllowEmptyStrings;
            }
            StringLengthAttribute stringLength = property.GetAttribute<StringLengthAttribute>();
            if (stringLength != null)
            {
                MaxLength = stringLength.MaximumLength;
                MinLength = stringLength.MinimumLength;
            }
            MaxLength = property.GetAttribute<MaxLengthAttribute>()?.Length;
            MinLength = property.GetAttribute<MinLengthAttribute>()?.Length;
            RangeAttribute range = property.GetAttribute<RangeAttribute>();
            if (range != null)
            {
                Range = new[] { range.Minimum, range.Maximum };
                Max = range.Maximum;
                Min = range.Minimum;
            }
            //枚举类型，作为数值类型返回
            if (property.PropertyType.IsEnum)
            {
                Type intType = typeof(int);
                TypeName = intType.FullName;
                Display = property.PropertyType.GetDescription();
                Type enumType = property.PropertyType;
                Array values = enumType.GetEnumValues();
                Enum[] enumItems = values.Cast<Enum>().ToArray();
                if (enumItems.Length > 0)
                {
                    EnumMetadatas = enumItems.Select(m => new EnumMetadata(m)).ToArray();
                }
            }
            else if (property.PropertyType == typeof(Guid))
            {
                TypeName = typeof(string).FullName;
            }
        }

        /// <summary>
        /// 获取或设置 属性名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 属性类型名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 获取或设置 显示名称
        /// </summary>
        public string Display { get; set; }

        /// <summary>
        /// 获取或设置 是否必须
        /// </summary>
        public bool? IsRequired { get; set; }

        /// <summary>
        /// 获取或设置 最大长度
        /// </summary>
        public int? MaxLength { get; set; }

        /// <summary>
        /// 获取或设置 最小长度
        /// </summary>
        public int? MinLength { get; set; }

        /// <summary>
        /// 获取或设置 取值范围
        /// </summary>
        public object[] Range { get; set; }

        /// <summary>
        /// 获取或设置 最大值
        /// </summary>
        public object Max { get; set; }

        /// <summary>
        /// 获取或设置 最小值
        /// </summary>
        public object Min { get; set; }

        /// <summary>
        /// 获取或设置 枚举元数据
        /// </summary>
        public EnumMetadata[] EnumMetadatas { get; set; }
    }
    ```
3. 枚举元数据：OSharp.CodeGenerator.EnumMetadata
    ```
    /// <summary>
    /// 枚举类型元数据
    /// </summary>
    public class EnumMetadata
    {
        /// <summary>
        /// 初始化一个<see cref="EnumMetadata"/>类型的新实例
        /// </summary>
        public EnumMetadata()
        { }

        /// <summary>
        /// 初始化一个<see cref="EnumMetadata"/>类型的新实例
        /// </summary>
        public EnumMetadata(Enum enumItem)
        {
            if (enumItem == null)
            {
                return;
            }
            Value = enumItem.CastTo<int>();
            Name = enumItem.ToString();
            Display = enumItem.ToDescription();
        }

        /// <summary>
        /// 获取或设置 枚举值
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// 获取或设置 枚举名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 显示名称
        /// </summary>
        public string Display { get; set; }
    }
    ```

### 元数据获取API
1. 实体类：/api/Common/GetTypeMetadatas?type=entity
2. 输入DTO：/api/Common/GetTypeMetadatas?type=inputdto
3. 输出DTO：/api/Common/GetTypeMetadatas?type=outputdto
4. 指定类型：/api/Common/GeTypeMetadata?typeFullName=`Liuliu.Demo.Identity.Entities.User`

## T4脚本编写示例

以输入DTO，输出DTO为源，生成TypeScript的列表组件基本代码

### 脚本执行者

```
<#@ template language="C#" debug="True" #>
<#@ output extension="txt" #>
<#@ include file="T4Toolbox.tt" #>
<#@ include file="TypeScript_Component_Template.tt" #>

<#@ assembly name="netstandard.dll" #>
<#@ assembly name="System.Core.dll" #>
<#@ assembly name="System.Net.Http.dll" #>
<#@ assembly name="$(TargetDir)Newtonsoft.Json.dll" #>
<#@ assembly name="$(TargetDir)OSharp.dll" #>

<#@ import namespace="System.IO" #>
<#@ import namespace="System.Linq" #>
<#@ Import Namespace="System.Text" #>
<#@ import namespace="System.Collections.Generic" #>
<#@ import namespace="System.Net.Http" #>
<#@ import namespace="OSharp.CodeGenerator" #>
<#@ import namespace="OSharp.Extensions" #>
<#@ import namespace="OSharp.Json" #>
<#
    string currentPath = Path.GetDirectoryName(Host.TemplateFile);
    string outputPath = currentPath + "\\ts_components";

    HttpClient client = new HttpClient();
    string url = "http://localhost:7001/api/Common/GetTypeMetadatas?type=inputdto";
    string json = client.GetStringAsync(url).Result;
    TypeMetadata[] inputs = json.FromJsonString<TypeMetadata[]>();
    url = "http://localhost:7001/api/Common/GetTypeMetadatas?type=outputdto";    
    json = client.GetStringAsync(url).Result;
    TypeMetadata[] outputs = json.FromJsonString<TypeMetadata[]>();
 
    foreach(var input in inputs)
    {
        string entityName = input.Name.Substring("", "InputDto");
        TypeMetadata output = outputs.FirstOrDefault(m=>m.Name == entityName+"OutputDto");

        TypeScriptComponentTemplate component = new TypeScriptComponentTemplate(entityName, input, output);
        string fileName = Path.Combine(outputPath, component.FileName);
        component.Output.Encoding = Encoding.UTF8;
        component.RenderToFile(fileName);
    }
#>
```

### TypeScript管理列表组件模板

```
<#+
// <copyright file="TypeScript_Component_Template.tt" company="">
//  Copyright © . All Rights Reserved.
// </copyright>

    public class TypeScriptComponentTemplate : CSharpTemplate
    {
        private TypeMetadata _inputDto;
        private TypeMetadata _outputDto;
        private string _entityName;
        private string _lowerEntityName;

        public TypeScriptComponentTemplate(string entityName, TypeMetadata inputDto, TypeMetadata outputDto)
        {
            _entityName = entityName;
            _lowerEntityName = entityName.UpperToLowerAndSplit();
            _inputDto = inputDto;
            _outputDto = outputDto;
        }

        public string FileName
        {
            get{ return string.Format("{0}.component.txt", _lowerEntityName); }
        }

	    public override string TransformText()
	    {
		    base.TransformText();
#>
import { Component, AfterViewInit, Injector, } from '@angular/core';
import { AuthConfig } from '@shared/osharp/osharp.model';
import { GridComponentBase } from '@shared/osharp/services/kendoui.service';

@Component({
  selector: 'admin-<#=_lowerEntityName #>',
  template: `<div id="grid-box-{{moduleName}}"></div>`
})

export class <#=_entityName #>Component extends GridComponentBase implements AfterViewInit {

  constructor(injector: Injector) {
    super(injector);
    this.moduleName = "<#=_entityName.LowerFirstChar() #>";
  }

  async ngAfterViewInit() {
    await this.checkAuth();
    if (this.auth.Read) {
      super.InitBase();
      super.ViewInitBase();
    } else {
      this.osharp.error("无权查看此页面");
    }
  }
  
  protected AuthConfig(): AuthConfig {
    return new AuthConfig("Root.Admin.Security.<#=_entityName #>", ["Read"]);
  }

  protected GetModel() {
    return {
      id: "Id",
      fields: {
<#+
        if(_inputDto.PropertyMetadatas != null)
        {
            foreach(var dto in _inputDto.PropertyMetadatas)
            {
#>
        <#=GetModelItem(dto) #>
<#+
            }
        }
 #>
      }
    }
  }

  protected GetGridColumns(): kendo.ui.GridColumn[] {
    let columns: kendo.ui.GridColumn[] = [
<#+
            if(_outputDto != null && _outputDto.PropertyMetadatas != null)
            {
#>
    {
      command: [
        { name: "destroy", iconClass: "k-icon k-i-delete", text: "" },
      ],
      width: 100
    }<#+
                foreach(var dto in _outputDto.PropertyMetadatas)
                {
                    if(dto.Name == "Updatable" || dto.Name == "Deletable")
                    {
                        continue;
                    }
#><#=GetColumn(dto) #><#+
                }
            }

 #>];
    return columns;
  }
}

<#+
            return this.GenerationEnvironment.ToString();
	    }
    }

    private static string GetModelItem(PropertyMetadata input)
    {
        string line= string.Format("{0}: {{ type: \"{1}\"", input.Name, GetSingleTypeName(input.TypeName));
        if(input.IsRequired != null && input.IsRequired.Value)
        {
            line += ", validation: { required: true }";
        }

        line+=" },";
        return line;
    }

    private static string GetColumn(PropertyMetadata output)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(string.Format(", {{\n\t  field: \"{0}\", title: \"{1}\"", output.Name, output.Display));
        if (output.TypeName == typeof(string).FullName)
        {
            sb.Append(",\n\t  filterable: this.osharp.data.stringFilterable");
        }
        if (output.TypeName == typeof(bool).FullName)
        {
            sb.Append(", width: 90");
            sb.Append(string.Format(",\n\t  template: d => this.kendoui.Boolean(d.{0})", output.Name));
            sb.Append(",\n\t  editor: (container, options) => this.kendoui.BooleanEditor(container, options)");
        }
        if (output.TypeName == typeof(int).FullName)
        {
            //枚举类型
            if (output.EnumMetadatas != null)
            {
                sb.Append(string.Format(",\n\t  template: d => this.osharp.valueToText(d.{0}, this.osharp.data.ENUM_SELECT_SOURCE),", output.Name));
                sb.Append(",\n\t  editor: (container, options) => this.kendoui.DropDownListEditor(container, options, this.osharp.data.ENUM_SELECT_SOURCE)");
                sb.Append(",\n\t  filterable: { ui: el => this.kendoui.DropDownList(el, this.osharp.data.ENUM_SELECT_SOURCE) }");
            }
        }
        if (output.TypeName == typeof(DateTime).FullName)
        {
            sb.Append(", width: 120");
            sb.Append(",\n      format: \"{0:yy-MM-dd HH:mm}\"");
        }
        sb.Append("\n\t}");
        return sb.ToString();
    }

    private static string GetSingleTypeName(string typeName)
    {
        switch(typeName)
        {
            case "System.Int32":
            case "System.Long":
                return "number";
            case "System.String":
                return "string";
            case "System.Boolean":
                return "boolean";
            case "System.DateTime":
                return "date";
            default:
                return "object";
        }
    }
#>

```