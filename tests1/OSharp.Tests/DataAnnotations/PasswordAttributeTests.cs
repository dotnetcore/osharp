// -----------------------------------------------------------------------
//  <copyright file="PasswordAttributeTests.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-07-26 14:38</last-date>
// -----------------------------------------------------------------------

using Shouldly;

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
                RequiredDigit = false,
                RequiredLowercase = false,
                RequiredUppercase = false
            };
            string name = "name";

            attr.IsValid(null).ShouldBeTrue();
            attr.IsValid("123").ShouldBeFalse();
            attr.FormatErrorMessage(name).ShouldContain("必须大于");
            attr.IsValid("123456").ShouldBeTrue();
            attr.IsValid("abcabc").ShouldBeTrue();

            attr.RequiredDigit = true;
            attr.IsValid("abcabc").ShouldBeFalse();
            attr.FormatErrorMessage(name).ShouldContain("必须包含数字");
            attr.IsValid("123456").ShouldBeTrue();
            attr.IsValid("abcabc").ShouldBeFalse();
            
            attr.CanOnlyDigit = false;
            attr.IsValid("123456").ShouldBeFalse();
            attr.FormatErrorMessage(name).ShouldContain("不允许是全是数字");
            attr.IsValid("123abc").ShouldBeTrue();

            attr.RequiredLowercase = true;
            attr.IsValid("123ABC").ShouldBeFalse();
            attr.FormatErrorMessage(name).ShouldContain("包含小写");
            attr.IsValid("123ABc").ShouldBeTrue();
            attr.IsValid("123abc").ShouldBeTrue();

            attr.RequiredUppercase = true;
            attr.IsValid("abc123").ShouldBeFalse();
            attr.FormatErrorMessage(name).ShouldContain("包含大写");
            attr.IsValid("123abC").ShouldBeTrue();
        }
    }
}