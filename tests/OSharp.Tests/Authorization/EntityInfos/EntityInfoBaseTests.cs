using Xunit;
using OSharp.Authorization.EntityInfos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using OSharp.Entity;
using OSharp.Json;

using Shouldly;

using Assert = Xunit.Assert;


namespace OSharp.Authorization.EntityInfos.Tests
{
    public class EntityInfoTests
    {
        [Fact]
        public void PropertiesTest()
        {
            EntityInfo entity = new EntityInfo();
            entity.Properties.ShouldNotBeNull();
            entity.Properties.ShouldBeEmpty();

            EntityProperty prop = new EntityProperty() { Name = "Name", TypeName = typeof(string).FullName };
            entity.PropertyJson = prop.ToJsonString();
            entity.Properties.ShouldNotBeNull();
            entity.Properties.ShouldBeEmpty();

            entity.PropertyJson = new List<EntityProperty>() { prop }.ToJsonString();
            entity.Properties.ShouldNotBeEmpty();
            entity.Properties.Length.ShouldBe(1);
            entity.Properties.First().Name.ShouldBe(prop.Name);
            entity.Properties.First().TypeName.ShouldBe(prop.TypeName);
        }

        [Fact()]
        public void FromTypeTest()
        {
            EntityInfo entity = new EntityInfo();
            Type type = null;
            Assert.Throws<ArgumentNullException>(() => entity.FromType(type));

            entity = new EntityInfo();
            type = typeof(TestEntityA);
            entity.FromType(type);

            entity.Name.ShouldBe("测试实体A");
            entity.TypeName.ShouldBe(typeof(TestEntityA).FullName + ",OSharp.Tests");

            entity.Properties.Length.ShouldBe(6);
            entity.Properties.ShouldContain(m => m.Name == "Id");
            entity.Properties.ShouldContain(m => m.Name == "Name");
            entity.Properties.ShouldContain(m => m.Name == "IsLocked");
            entity.Properties.ShouldContain(m => m.Name == "CreatedTime");
            entity.Properties.ShouldNotContain(m => m.Name == "DeletedTime");
            entity.Properties.First(m => m.Name == "Id").TypeName.ShouldBe(typeof(int).FullName);

            entity.Properties.First(m => m.Name == "Enum").ValueRange.Count.ShouldBe(4);
            entity.Properties.First(m => m.Name == "OtherId").IsUserFlag.ShouldBeTrue();
        }


        [Description("测试实体A")]
        private class TestEntityA : EntityBase<int>, ILockable, ISoftDeletable, ICreatedTime
        {
            public string Name { get; set; }

            public MyEnum Enum { get; set; }

            [UserFlag]
            public int OtherId { get; set; }

            /// <summary>
            /// 获取或设置 是否锁定当前信息
            /// </summary>
            public bool IsLocked { get; set; }

            /// <summary>
            /// 获取或设置 数据逻辑删除时间，为null表示正常数据，有值表示已逻辑删除，同时删除时间每次不同也能保证索引唯一性
            /// </summary>
            public DateTime? DeletedTime { get; set; }

            /// <summary>
            /// 获取或设置 创建时间
            /// </summary>
            public DateTime CreatedTime { get; set; }
        }

        private enum MyEnum
        {
            EnumItem1,
            EnumItem2,
            EnumItem3,
            EnumItem4,
        }
    }
}