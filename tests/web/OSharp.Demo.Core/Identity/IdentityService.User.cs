// -----------------------------------------------------------------------
//  <copyright file="IdentityService.User.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-18 14:47</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;

using OSharp.Demo.Identity.Entities;


namespace OSharp.Demo.Identity
{
    public partial class IdentityService
    {
        /// <inheritdoc />
        public IQueryable<User> Users(params Expression<Func<User, object>>[] propertySelectors)
        {
            return _useRepository.Query(propertySelectors);
        }
    }
}