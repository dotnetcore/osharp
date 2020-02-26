// -----------------------------------------------------------------------
//  <copyright file="DataAuthorizationPack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-27 0:35</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using Liuliu.Demo.Authorization.Dtos;
using Liuliu.Demo.Authorization.Entities;

using OSharp.Authorization;
using OSharp.Authorization.Dtos;
using OSharp.Authorization.EntityInfos;


namespace Liuliu.Demo.Authorization
{
    public class DataAuthorizationPack
        : DataAuthorizationPackBase<DataAuthManager, DataAuthCache, EntityInfo, EntityInfoInputDto, EntityRole, EntityRoleInputDto, int>
    { }
}