// -----------------------------------------------------------------------
//  <copyright file="AutoMapperConfiguration.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-04 0:24</last-date>
// -----------------------------------------------------------------------

using AutoMapper;
using AutoMapper.Configuration;

using Liuliu.Demo.Identity.Entities;

using OSharp.AutoMapper;


namespace Liuliu.Demo.Identity.Dtos
{
    /// <summary>
    /// DTO对象映射配置
    /// </summary>
    public class AutoMapperConfiguration : AutoMapperTupleBase
    {
        /// <summary>
        /// 创建对象映射
        /// </summary>
        public override void CreateMap()
        {
            CreateMap<Role, RoleNode>().ForMember(rn => rn.RoleId, opt => opt.MapFrom(r => r.Id))
                .ForMember(rn => rn.RoleName, opt => opt.MapFrom(r => r.Name));
        }
    }
}