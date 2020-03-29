// -----------------------------------------------------------------------
//  <copyright file="DateTimeExtensionsTests.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-05-05 11:49</last-date>
// -----------------------------------------------------------------------

using System;

using Xunit;


namespace OSharp.Timing.Tests
{

    public class DateTimeExtensionsTests
    {
        [Fact()]
        public void IsWeekendTest()
        {
            DateTime dt = new DateTime(2015, 5, 2);
            Assert.True(dt.IsWeekend());
            dt = new DateTime(2015, 5, 3);
            Assert.True(dt.IsWeekend());
            for (int i = 0; i < 5; i++)
            {
                dt = new DateTime(2015, 5, 4 + i);
                Assert.False(dt.IsWeekend());
            }
        }

        [Fact()]
        public void IsWeekdayTest()
        {
            DateTime dt = new DateTime(2015, 5, 2);
            Assert.False(dt.IsWeekday());
            dt = new DateTime(2015, 5, 3);
            Assert.False(dt.IsWeekday());
            for (int i = 0; i < 5; i++)
            {
                dt = new DateTime(2015, 5, 4 + i);
                Assert.True(dt.IsWeekday());
            }
        }

    }
}