using System;
using System.Collections.Generic;
using System.Text;

using OSharp.CodeGeneration.Schema;

using Shouldly;

using Xunit;


namespace OSharp.CodeGeneration.Tests.Schema
{
    public class TypeHelperTests
    {
        [Fact]
        public void ToSingleTypeNameTest()
        {
            string fullName = "System.String";
            TypeHelper.ToSingleTypeName(fullName).ShouldBe("string");
            fullName = "System.Guid";
            TypeHelper.ToSingleTypeName(fullName, true).ShouldBe("Guid?");
            fullName = "string";
            TypeHelper.ToSingleTypeName(fullName).ShouldBe("string");
        }
    }
}
