// -----------------------------------------------------------------------
//  <copyright file="AbstractBuilder.cs" company="OSharp开源团队">
//      Copyright (c) 2014 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2014:07:08 17:11</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;
using Xunit;


namespace OSharp.Extensions.Tests
{
    
    public class EnumExtensionsTests
    {
        [Fact()]
        public void ToDescriptionTest()
        {
            TestEnum value = TestEnum.EnumItemA;
            Assert.Equal("枚举项A", value.ToDescription());

            value = TestEnum.EnumItemB;
            Assert.Equal("EnumItemB", value.ToDescription());
        }


        private enum TestEnum
        {
            [Description("枚举项A")] 
            EnumItemA,
            EnumItemB
        }


    }
}