using Xunit;
using OSharp.Audits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Shouldly;


namespace OSharp.Audits.Tests
{
    public class AuditEntityEventDataTests
    {
        [Fact()]
        public void AuditEntityEventDataTest()
        {
            var list = new List<AuditEntityEntry>() { new AuditEntityEntry() };
            AuditEntityEventData eventData = new AuditEntityEventData(list);
            eventData.AuditEntities.ShouldNotBeNull();
            eventData.AuditEntities.ShouldNotBeEmpty();
            eventData.AuditEntities.First().ShouldBe(list[0]);
        }
    }
}