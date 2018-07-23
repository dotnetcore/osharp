using System;

using Xunit;
using Shouldly;

namespace OSharp.Timing.Tests
{
    public class DateTimeRangeTests
    {
        [Fact()]
        public void DateTimeRangeTest_Ctor()
        {
            DateTimeRange range = new DateTimeRange();
            Assert.Equal(range.StartTime, DateTime.MinValue);
            Assert.Equal(range.EndTime, DateTime.MaxValue);

            DateTime now = new DateTime(2015, 8, 7, 11, 15, 22);
            range = new DateTimeRange(now.Date.AddDays(-1), now.Date.AddDays(2));
            Assert.Equal(6, range.StartTime.Day);
            Assert.Equal(9, range.EndTime.Day);
        }

        [Fact()]
        public void DateTimeRangeTest_Properties()
        {
            DateTimeRange.Today.StartTime.ShouldBeGreaterThan(DateTimeRange.Yesterday.EndTime);
            DateTimeRange.Today.EndTime.ShouldBeLessThan(DateTimeRange.Tomorrow.StartTime);

            DateTimeRange.ThisMonth.StartTime.Day.ShouldBe(1);
            DateTimeRange.LastMonth.StartTime.Day.ShouldBe(1);
            DateTimeRange.NextMonth.StartTime.Day.ShouldBe(1);

            DateTimeRange.ThisMonth.StartTime.ShouldBeGreaterThan(DateTimeRange.LastMonth.EndTime);
            DateTimeRange.ThisMonth.EndTime.ShouldBeLessThan(DateTimeRange.NextMonth.EndTime);

            DateTimeRange.ThisYear.StartTime.Month.ShouldBe(1);
            DateTimeRange.ThisYear.StartTime.Day.ShouldBe(1);

            DateTimeRange.ThisYear.StartTime.ShouldBeGreaterThan(DateTimeRange.LastYear.EndTime);
            DateTimeRange.ThisYear.EndTime.ShouldBeLessThan(DateTimeRange.NextYear.StartTime);

            DateTimeRange.Last7DaysExceptToday.EndTime.ShouldBeLessThan(DateTimeRange.Today.StartTime);
            DateTimeRange.Last30DaysExceptToday.EndTime.ShouldBeLessThan(DateTimeRange.Today.StartTime);
        }

    }
}
