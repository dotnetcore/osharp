using OSharp.Timing;
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

        [Fact()]
        public void ToUniqueStringTest()
        {
            DateTime now = new DateTime(2015, 11, 4, 15, 10, 25);
            Assert.Equal("1530854625", now.ToUniqueString());
            Assert.Equal("1530854625000", now.ToUniqueString(true));
        }

        [Fact()]
        public void ToUtcTimeTest()
        {
            Assert.Equal(new DateTime(2000, 1, 1), new DateTime(2000, 1, 1, 8, 0, 0).ToUtcTime());
        }

        [Fact()]
        public void FromUtcTimeTest()
        {
            Assert.Equal(new DateTime(2000, 1, 1, 8, 0, 0), new DateTime(2000, 1, 1).FromUtcTime());
        }

        [Fact()]
        public void ToJsGetTimeTest()
        {
            Assert.Equal("0", new DateTime(1970, 1, 1, 8, 0, 0).ToJsGetTime(false));
            Assert.Equal("0", new DateTime(1970, 1, 1, 8, 0, 0).ToJsGetTime());
            Assert.Equal("1500000000", new DateTime(2017, 7, 14, 10, 40, 0).ToJsGetTime(false));
            Assert.Equal("1500000000000", new DateTime(2017, 7, 14, 10, 40, 0).ToJsGetTime());
        }

        [Fact()]
        public void FromJsGetTimeTest()
        {
            DateTime now = new DateTime(2017, 7, 14, 10, 40, 0);
            Assert.Equal(((long)1500000000).FromJsGetTime(), now);
        }

    }
}