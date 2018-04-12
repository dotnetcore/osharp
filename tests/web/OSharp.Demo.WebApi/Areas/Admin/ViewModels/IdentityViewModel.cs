// -----------------------------------------------------------------------
//  <copyright file="IdentityViewModel.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-04-09 21:01</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.Demo.WebApi.Areas.Admin.ViewModels
{
    public class UserSetPermissionModel
    {
        public int UserId { get; set; }

        public int[] RoleIds { get; set; }

        public int[] ModuleIds { get; set; }
    }


    public class RoleSetPermissionModel
    {
        public int RoleId { get; set; }

        public int[] ModuleIds { get; set; }
    }


    public class ModuleSetFunctionsModel
    {
        public int ModuleId { get; set; }

        public Guid[] FunctionIds { get; set; }
    }
}