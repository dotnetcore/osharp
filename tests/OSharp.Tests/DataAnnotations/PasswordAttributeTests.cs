// -----------------------------------------------------------------------
//  <copyright file="PasswordAttributeTests.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-07-26 14:38</last-date>
// -----------------------------------------------------------------------

using Xunit;


namespace OSharp.DataAnnotations.Tests
{
    
    public class PasswordAttributeTests
    {
        [Fact()]
        public void IsValidTest()
        {
            PasswordAttribute attr = new PasswordAttribute()
            {
                RequiredLength = 6,
                CanOnlyDigit = true,
                RequiredDigit = true,
                RequiredLowercase = false,
                RequiredUppercase = false
            };
            Assert.True(attr.IsValid(null));
            Assert.False(attr.IsValid("213"));
            Assert.True(attr.IsValid("123456"));
            attr.CanOnlyDigit = false;
            Assert.False(attr.IsValid("12356"));
            Assert.True(attr.IsValid("123abc"));
            attr.RequiredLowercase = true;
            Assert.False(attr.IsValid("123ABC"));
            Assert.True(attr.IsValid("123abc"));
            attr.RequiredUppercase = true;
            Assert.False(attr.IsValid("123abc"));
            Assert.False(attr.IsValid("123ABC"));
            Assert.True(attr.IsValid("123AbC"));
        }
    }
}