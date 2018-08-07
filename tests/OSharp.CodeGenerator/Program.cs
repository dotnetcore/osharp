using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using OSharp.Collections;
using OSharp.Extensions;


namespace OSharp.CodeGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string json =
                "{\"Name\":\"IsLocked\",\"TypeName\":\"System.Boolean\",\"Display\":\"IsLocked\",\"IsRequired\":null,\"MaxLength\":null,\"MinLength\":null,\"Range\":null,\"Max\":null,\"Min\":null,\"EnumMetadatas\":null}";
            PropertyMetadata output = json.FromJsonString<PropertyMetadata>();

            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("{{ field: \"{0}\", title: \"{1}\"", output.Name, output.Display));
            if (output.TypeName == typeof(string).FullName)
            {
                sb.Append(",\n  filterable: this.osharp.data.stringFilterable");
            }
            if (output.TypeName == typeof(bool).FullName)
            {
                sb.Append(string.Format(",\n  template: d => this.kendoui.Boolean(d.{0})", output.Name));
                sb.Append(",\n  editor: (container, options) => this.kendoui.BooleanEditor(container, options)");
            }
            if (output.TypeName == typeof(int).FullName)
            {
                //枚举类型
                if (output.EnumMetadatas != null)
                {
                    sb.Append(string.Format(",\n  template: d => this.osharp.valueToText(d.{0}, this.osharp.data.ENUM_SELECT_SOURCE),", output.Name));
                    sb.Append(",\n  editor: (container, options) => this.kendoui.DropDownListEditor(container, options, this.osharp.data.ENUM_SELECT_SOURCE)");
                    sb.Append(",\n  filterable: { ui: el => this.kendoui.DropDownList(el, this.osharp.data.ENUM_SELECT_SOURCE) }");
                }
            }
            if (output.TypeName == typeof(DateTime).FullName)
            {
                sb.Append(",\n  format: \"{0:yy-MM-dd HH:mm}\"");
            }
            sb.Append("];");

            Console.WriteLine(sb.ToString());
        }

        
    }
}
