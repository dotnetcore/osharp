using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using OSharp.Demo.Identity.Entities;
using OSharp.Entity;


namespace OSharp.Demo.EntityConfiguration.Identity
{
    public class RoleConfiguration : EntityTypeConfigurationBase<Role, int>
    {
        #region Overrides of EntityTypeConfigurationBase<Role,int>

        /// <summary>
        /// 重写以实现实体类型各个属性的数据库配置
        /// </summary>
        /// <param name="builder">实体类型创建器</param>
        public override void Configure(EntityTypeBuilder<Role> builder)
        {

        }

        #endregion
    }
}
