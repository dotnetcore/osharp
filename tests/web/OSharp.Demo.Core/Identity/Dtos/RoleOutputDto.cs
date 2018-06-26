// -----------------------------------------------------------------------
//  <copyright file="RoleOutputDto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-26 14:56</last-date>
// -----------------------------------------------------------------------

using System;

using OSharp.Entity;


namespace OSharp.Demo.Identity.Dtos
{
    public class RoleOutputDto : IOutputDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Remark { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsDefault { get; set; }

        public bool IsLocked { get; set; }

        public DateTime CreatedTime { get; set; }
    }
}