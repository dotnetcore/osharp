using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using OSharp.Collections;
using OSharp.Extensions;


namespace OSharp.CodeGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            bool exit = false;
            while (true)
            {
                try
                {
                    Console.WriteLine(@"请输入命令：0; 退出程序，功能命令：1 - n");
                    string input = Console.ReadLine();
                    if (input == null)
                    {
                        continue;
                    }
                    switch (input.ToLower())
                    {
                        case "0":
                            exit = true;
                            break;
                        case "1":
                            Method01();
                            break;
                        case "2":
                            Method02();
                            break;
                        case "3":
                            Method03();
                            break;
                        case "4":
                            Method04();
                            break;
                        case "5":
                            Method05();
                            break;
                        case "6":
                            Method06();
                            break;
                        case "7":
                            Method07();
                            break;
                        case "8":
                            Method08();
                            break;
                        case "9":
                            Method09();
                            break;
                        case "10":
                            Method10();
                            break;
                        case "11":
                            Method11();
                            break;
                    }
                    if (exit)
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private static void Method01()
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

        private static void Method02()
        {
            List<Guid>ids = new List<Guid>();
            for (int i = 0; i < 10000; i++)
            {
                Thread thread = null;
                thread = new Thread(() =>
                {
                    ids.Add(Guid.NewGuid());
                });
                thread.Start();
            }
            Thread.Sleep(100);
            Console.WriteLine(ids.Count);
            Console.WriteLine(ids.Distinct().Count());
        }
        
        private static void Method03()
        {
            throw new NotImplementedException();
        }

        private static void Method04()
        {
            throw new NotImplementedException();
        }

        private static void Method05()
        {
            throw new NotImplementedException();
        }

        private static void Method06()
        {
            throw new NotImplementedException();
        }

        private static void Method07()
        {
            throw new NotImplementedException();
        }

        private static void Method08()
        {
            throw new NotImplementedException();
        }

        private static void Method09()
        {
            throw new NotImplementedException();
        }

        private static void Method10()
        {
            throw new NotImplementedException();
        }

        private static void Method11()
        {
            throw new NotImplementedException();
        }

        private static string GetModelItemValidation(PropertyMetadata inProp)
        {
            if (inProp == null || !inProp.HasValidateAttribute())
            {
                return null;
            }
            string line = ", validation: { ";

            List<string> list = new List<string>();
            if (inProp.IsRequired != null && inProp.IsRequired.Value)
            {
                list.Add("required: true");
            }
            if (inProp.MinLength != null)
            {
                list.Add("minLength: " + inProp.MinLength.Value);
            }
            if (inProp.MaxLength != null)
            {
                list.Add("maxLength: " + inProp.MaxLength.Value);
            }
            if (inProp.Min != null)
            {
                list.Add("min: " + inProp.Min);
            }
            if (inProp.Max != null)
            {
                list.Add("max: " + inProp.Max);
            }
            if (inProp.Range != null)
            {
                list.Add("min: " + inProp.Range[0]);
                list.Add("max: " + inProp.Range[1]);
            }

            line += list.ExpandAndToString(", ");

            line += " }";
            return line;
        }

    }
}
