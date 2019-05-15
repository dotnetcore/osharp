// -----------------------------------------------------------------------
//  <copyright file="DashedRoutingConvention.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-03-18 12:59</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;


namespace OSharp.AspNetCore.Mvc.Conventions
{
    /// <summary>
    /// 单词减号分隔符(AddUser -> add-user) URL转换器
    /// </summary>
    public class DashedRoutingConvention : IControllerModelConvention
    {
        /// <summary>
        /// Called to apply the convention to the <see cref="T:Microsoft.AspNetCore.Mvc.ApplicationModels.ControllerModel" />.
        /// </summary>
        /// <param name="controller">The <see cref="T:Microsoft.AspNetCore.Mvc.ApplicationModels.ControllerModel" />.</param>
        public void Apply(ControllerModel controller)
        {
            var hasRouteAttribute = controller.Selectors.Any(selector => selector.AttributeRouteModel != null);
            if (hasRouteAttribute)
            {
                return;
            }
            foreach (ActionModel action in controller.Actions)
            {
                foreach (SelectorModel model in action.Selectors.Where(m=>m.AttributeRouteModel == null))
                {
                    List<string> parts = new List<string>();
                    foreach (var attribute in controller.Attributes)
                    {
                        if (attribute is AreaAttribute area)
                        {
                            parts.Add(PascalToKebabCase(area.RouteValue));
                            break;
                        }
                    }

                    if (parts.Count == 0 && controller.ControllerName == "Home" && action.ActionName == "Index")
                    {
                        continue;
                    }
                    parts.Add(PascalToKebabCase(controller.ControllerName));

                    if (action.ActionName != "Index")
                    {
                        parts.Add(PascalToKebabCase(action.ActionName));
                    }
                    
                    string template = string.Join("/", parts);
                    model.AttributeRouteModel = new AttributeRouteModel() { Template = template };
                }
            }
        }

        private static string PascalToKebabCase(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            return Regex.Replace(value, "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])", "-$1", RegexOptions.Compiled).Trim().ToLower();
        }
    }
}