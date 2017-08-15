using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

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
            Assert.Equal(range.StartTime.Day, 6);
            Assert.Equal(range.EndTime.Day, 9);
        }

        //[Fact()]
        //public void DateTimeRangeTest_Properties()
        //{
        //    DateTime now = new DateTime(2015, 8, 7, 11, 15, 22);
        //    Smock.Run(context =>
        //    {
        //        context.Setup(() => DateTime.Now).Returns(now);

        //        Assert.Equal(DateTimeRange.Yesterday.StartTime, new DateTime(2015, 8, 6));
        //        Assert.Equal(DateTimeRange.Yesterday.EndTime, new DateTime(2015, 8, 7).AddMilliseconds(-1));

        //        Assert.Equal(DateTimeRange.Today.StartTime, new DateTime(2015, 8, 7));
        //        Assert.Equal(DateTimeRange.Today.EndTime, new DateTime(2015, 8, 8).AddMilliseconds(-1));

        //        Assert.Equal(DateTimeRange.Tomorrow.StartTime, new DateTime(2015, 8, 8));
        //        Assert.Equal(DateTimeRange.Tomorrow.EndTime, new DateTime(2015, 8, 9).AddMilliseconds(-1));

        //        Assert.Equal(DateTimeRange.LastWeek.StartTime, new DateTime(2015, 7, 26));
        //        Assert.Equal(DateTimeRange.LastWeek.EndTime, new DateTime(2015, 8, 2).AddMilliseconds(-1));

        //        Assert.Equal(DateTimeRange.ThisWeek.StartTime, new DateTime(2015, 8, 2));
        //        Assert.Equal(DateTimeRange.ThisWeek.EndTime, new DateTime(2015, 8, 9).AddMilliseconds(-1));

        //        Assert.Equal(DateTimeRange.NextWeek.StartTime, new DateTime(2015, 8, 9));
        //        Assert.Equal(DateTimeRange.NextWeek.EndTime, new DateTime(2015, 8, 16).AddMilliseconds(-1));

        //        Assert.Equal(DateTimeRange.LastMonth.StartTime, new DateTime(2015, 7, 1));
        //        Assert.Equal(DateTimeRange.LastMonth.EndTime, new DateTime(2015, 8, 1).AddMilliseconds(-1));

        //        Assert.Equal(DateTimeRange.ThisMonth.StartTime, new DateTime(2015, 8, 1));
        //        Assert.Equal(DateTimeRange.ThisMonth.EndTime, new DateTime(2015, 9, 1).AddMilliseconds(-1));

        //        Assert.Equal(DateTimeRange.NextMonth.StartTime, new DateTime(2015, 9, 1));
        //        Assert.Equal(DateTimeRange.NextMonth.EndTime, new DateTime(2015, 10, 1).AddMilliseconds(-1));

        //        Assert.Equal(DateTimeRange.LastYear.StartTime, new DateTime(2014, 1, 1));
        //        Assert.Equal(DateTimeRange.LastYear.EndTime, new DateTime(2015, 1, 1).AddMilliseconds(-1));

        //        Assert.Equal(DateTimeRange.ThisYear.StartTime, new DateTime(2015, 1, 1));
        //        Assert.Equal(DateTimeRange.ThisYear.EndTime, new DateTime(2016, 1, 1).AddMilliseconds(-1));

        //        Assert.Equal(DateTimeRange.NextYear.StartTime, new DateTime(2016, 1, 1));
        //        Assert.Equal(DateTimeRange.NextYear.EndTime, new DateTime(2017, 1, 1).AddMilliseconds(-1));

        //        Assert.Equal(DateTimeRange.Last30Days.StartTime, new DateTime(2015, 7, 8, 11, 15, 22));
        //        Assert.Equal(DateTimeRange.Last30Days.EndTime, now);

        //        Assert.Equal(DateTimeRange.Last7Days.StartTime, new DateTime(2015, 7, 31, 11, 15, 22));
        //        Assert.Equal(DateTimeRange.Last7Days.EndTime, now);
        //    });

        //}

    }
}
