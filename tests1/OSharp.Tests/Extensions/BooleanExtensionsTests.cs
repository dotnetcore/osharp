// -----------------------------------------------------------------------
//  <copyright file="AbstractBuilder.cs" company="OSharp开源团队">
//      Copyright (c) 2014 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2014:07:04 9:31</last-date>
// -----------------------------------------------------------------------

using Xunit;


namespace OSharp.Extensions.Tests
{
    
    public class BooleanExtensionsTests
    {
        [Fact()]
        public void ToLowerTest()
        {
            Assert.Equal("true", true.ToLower());
            Assert.Equal("false", false.ToLower());
            Assert.NotEqual("True", true.ToLower());
            Assert.NotEqual("False", false.ToLower());
        }
    }
}