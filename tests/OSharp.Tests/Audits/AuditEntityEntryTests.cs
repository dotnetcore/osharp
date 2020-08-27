using Xunit;
using OSharp.Audits;
using System;
using System.Collections.Generic;
using System.Text;

using Shouldly;


namespace OSharp.Audits.Tests
{
    public class AuditEntityEntryTests
    {
        [Fact()]
        public void AuditEntityEntryTest()
        {
            AuditEntityEntry entry = new AuditEntityEntry();
            entry.Name.ShouldBeNull();
            entry.PropertyEntries.ShouldNotBeNull();
            entry.PropertyEntries.ShouldBeEmpty();

            entry = new AuditEntityEntry("name", "typeName", OperateType.Delete);
            entry.Name.ShouldBe("name");
            entry.TypeName.ShouldBe("typeName");
            entry.OperateType.ShouldBe(OperateType.Delete);
            entry.PropertyEntries.ShouldNotBeNull();
            entry.PropertyEntries.ShouldBeEmpty();
        }
    }
}