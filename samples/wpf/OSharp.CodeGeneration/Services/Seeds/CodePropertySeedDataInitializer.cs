// -----------------------------------------------------------------------
//  <copyright file="CodePropertySeedDataInitializer.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-09 18:09</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Microsoft.Extensions.DependencyInjection;

using OSharp.CodeGeneration.Services.Entities;
using OSharp.Entity;


namespace OSharp.CodeGeneration.Services.Seeds
{
    public class CodePropertySeedDataInitializer : SeedDataInitializerBase<CodeProperty, Guid>
    {
        /// <summary>
        /// 初始化一个<see cref="T:OSharp.Entity.SeedDataInitializerBase`2" />类型的新实例
        /// </summary>
        public CodePropertySeedDataInitializer(IServiceProvider rootProvider)
            : base(rootProvider)
        { }

        /// <summary>获取 种子数据初始化的顺序</summary>
        public override int Order => 5;

        /// <summary>重写以提供要初始化的种子数据</summary>
        /// <returns></returns>
        protected override CodeProperty[] SeedData(IServiceProvider provider)
        {
            IRepository<CodeEntity, Guid> repository = provider.GetRequiredService<IRepository<CodeEntity, Guid>>();
            CodeEntity entity = repository.GetFirst(m => m.Name == "User");
            List<CodeProperty> properties = new List<CodeProperty>()
            {
                new CodeProperty()
                {
                    Name = "UserName", Display = "用户名", TypeName = "System.String", Updatable = true, Sortable = true, Filterable = true, Order = 1,
                    IsRequired = true, MaxLength = 200, EntityId = entity.Id
                },
                new CodeProperty()
                {
                    Name = "NickName", Display = "用户昵称", TypeName = "System.String", Order = 2, IsRequired = true, MaxLength = 200,
                    EntityId = entity.Id
                },
                new CodeProperty() { Name = "Email", Display = "邮箱", TypeName = "System.String", Order = 3, MaxLength = 200, EntityId = entity.Id },
                new CodeProperty() { Name = "EmailConfirmed", Display = "邮箱确认", TypeName = "System.Boolean", Order = 4, EntityId = entity.Id },
                new CodeProperty() { Name = "PhoneNumber", Display = "手机号", TypeName = "System.String", Order = 5, MaxLength = 50, EntityId = entity.Id },
                new CodeProperty() { Name = "PhoneNumberConfirmed", Display = "手机号确认", TypeName = "System.Boolean", Order = 6, EntityId = entity.Id },
            };
            
            return properties.ToArray();
        }

        /// <summary>重写以提供判断某个实体是否存在的表达式</summary>
        /// <param name="entity">要判断的实体</param>
        /// <returns></returns>
        protected override Expression<Func<CodeProperty, bool>> ExistingExpression(CodeProperty entity)
        {
            return m => m.Name == entity.Name && m.EntityId == entity.EntityId;
        }
    }
}
