using Xunit;
using OSharp.Audits;
using System;
using System.Collections.Generic;
using System.Text;

using Shouldly;


namespace OSharp.Audits.Tests
{
    public class AuditOperationEntryTests
    {
        [Fact()]
        public void AuditOperationEntryTest()
        {
            AuditOperationEntry operation = new AuditOperationEntry();
            operation.EntityEntries.ShouldNotBeNull();
            operation.EntityEntries.ShouldBeEmpty();
        }
    }
}