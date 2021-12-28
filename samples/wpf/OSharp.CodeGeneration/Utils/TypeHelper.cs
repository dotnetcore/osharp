// -----------------------------------------------------------------------
//  <copyright file="TypeHelper.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-08 19:44</last-date>
// -----------------------------------------------------------------------

using OSharp.Data;


namespace OSharp.CodeGeneration.Utils
{
    /// <summary>
    /// 类型辅助操作
    /// </summary>
    public static class TypeHelper
    {
        public static string GetNamespace(string fullName)
        {
            int index = fullName.LastIndexOf('.');
            if (index > 0)
            {
                return fullName.Substring(0, index);
            }

            return null;
        }

        /// <summary>
        /// 获取属性表示类型的简单类型，如 System.String 返回 string
        /// </summary>
        public static string ToSingleTypeName(string fullName, bool isNullable = false)
        {
            Check.NotNull(fullName, nameof(fullName));
            int index = fullName.LastIndexOf('.');
            string name = fullName.Substring(index + 1);
            switch (fullName)
            {
                case "System.Byte":
                    name = "byte";
                    break;
                case "System.Int32":
                    name = "int";
                    break;
                case "System.Int64":
                    name = "long";
                    break;
                case "System.Decimal":
                    name = "decimal";
                    break;
                case "System.Single":
                    name = "float";
                    break;
                case "System.Double":
                    name = "double";
                    break;
                case "System.String":
                    name = "string";
                    break;
                case "System.Guid":
                    name = "Guid";
                    break;
                case "System.Boolean":
                    name = "bool";
                    break;
                case "System.DateTime":
                    name = "DateTime";
                    break;
            }

            if (isNullable)
            {
                name = name + "?";
            }

            return name;
        }
    }
}
