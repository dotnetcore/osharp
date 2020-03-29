using Xunit;
using OSharp.Authorization.EntityInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OSharp.Json;

using Shouldly;


namespace OSharp.Authorization.EntityInfos.Tests
{
    public class EntityInfoBaseTests
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

            entity.PropertyJson = new List<EntityProperty>(){prop}.ToJsonString();
            entity.Properties.ShouldNotBeEmpty();
            entity.Properties.Length.ShouldBe(1);
            entity.Properties.First().Name.ShouldBe(prop.Name);
            entity.Properties.First().TypeName.ShouldBe(prop.TypeName);
        }


        [Fact()]
        public void FromTypeTest()
        {
            //Assert.True(false, "This test needs an implementation");
        }
    }
}