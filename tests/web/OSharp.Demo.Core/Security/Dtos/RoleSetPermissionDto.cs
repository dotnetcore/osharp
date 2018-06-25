// -----------------------------------------------------------------------
//  <copyright file="RoleSetPermissionDto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-25 21:18</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Demo.Security.Dtos
{
    public class RoleSetPermissionDto
    {
        public int RoleId { get; set; }

        public int[] ModuleIds { get; set; }
    }
}