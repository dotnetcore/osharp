using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OSharp.Entity;
using OSharp.MultiTenancy;
using System;

namespace Liuliu.Demo.EntityConfiguration
{
    /// <summary>
    /// 租户实体类型配置
    /// </summary>
    public partial class TenantEntityConfiguration : EntityTypeConfigurationBase<TenantEntity, Guid>
    {
        public override Type DbContextType { get; } = typeof(TenantDbContext);

        /// <summary>
        /// 重写以实现实体类型各个属性的数据库配置
        /// </summary>
        /// <param name="builder">实体类型构建器</param>
        public override void Configure(EntityTypeBuilder<TenantEntity> builder)
        {
            // 配置表名
            builder.ToTable("Tenants");

            // 配置属性
            builder.Property(m => m.TenantId)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("租户ID");

            builder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasComment("租户名称");

            builder.Property(m => m.Host)
                .IsRequired()
                .HasMaxLength(100)
                .HasComment("租户主机");

            builder.Property(m => m.ConnectionString)
                .HasMaxLength(1000)
                .HasComment("连接字符串");

            builder.Property(m => m.IsEnabled)
                .IsRequired()
                .HasDefaultValue(true)
                .HasComment("是否启用");

            builder.Property(m => m.CreatedTime)
                .IsRequired()
                .HasComment("创建时间");

            builder.Property(m => m.UpdatedTime)
                .HasComment("更新时间");

            // 配置索引
            builder.HasIndex(m => m.TenantId)
                .IsUnique()
                .HasDatabaseName("IX_Tenants_TenantId");

            builder.HasIndex(m => m.Host)
                .HasDatabaseName("IX_Tenants_Host");

            EntityConfigurationAppend(builder);
        }

        /// <summary>
        /// 额外的数据映射
        /// </summary>
        partial void EntityConfigurationAppend(EntityTypeBuilder<TenantEntity> builder);
    }
} 