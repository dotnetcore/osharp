using System;
using System.Linq;
using Xunit;

namespace OSharp.Maths.Tests
{
    public class AnyRadixConvertTests
    {
        [Fact()]
        public void X2HTest()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => AnyRadixConvert.X2H("123", 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => AnyRadixConvert.X2H("123", 63));
            Assert.Throws<ArgumentException>(() => AnyRadixConvert.X2H("", 2));
            Assert.Throws<ArgumentException>(() => AnyRadixConvert.X2H("5", 2));
            Assert.Equal(AnyRadixConvert.X2H("0", 2), 0UL);
            Assert.Equal(AnyRadixConvert.X2H("10", 2), 2UL);
            Assert.Equal(AnyRadixConvert.X2H("10", 10), 10UL);
            Assert.Equal(AnyRadixConvert.X2H("A", 16), 10UL);
        }

        [Fact()]
        public void H2XTest()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => AnyRadixConvert.H2X(123UL, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => AnyRadixConvert.H2X(123UL, 63));
            Assert.Equal(AnyRadixConvert.H2X(5UL, 2), "101");
            Assert.Equal(AnyRadixConvert.H2X(0, 2), "0");
            Assert.Equal(AnyRadixConvert.H2X(2, 2), "10");
            Assert.Equal(AnyRadixConvert.H2X(10, 10), "10");
            Assert.Equal(AnyRadixConvert.H2X(10, 16), "A");
        }

        [Fact()]
        public void X2XTest()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => AnyRadixConvert.X2X("0", 0, 10));
            Assert.Throws<ArgumentOutOfRangeException>(() => AnyRadixConvert.X2X("0", 10, 63));
            Assert.Throws<ArgumentException>(() => AnyRadixConvert.X2X("2", 2, 10));
            Assert.Throws<ArgumentException>(() => AnyRadixConvert.X2X("A", 10, 10));
            Assert.Throws<ArgumentException>(() => AnyRadixConvert.X2X("G", 16, 10));
            Assert.Equal(AnyRadixConvert.X2X("0", 2, 10), "0");
            Assert.Equal(AnyRadixConvert.X2X("100", 2, 10), "4");
            Assert.Equal(AnyRadixConvert.X2X("100", 10, 16), "64");
            Assert.Equal(AnyRadixConvert.X2X("100", 16, 10), "256");
        }

        [Fact()]
        public void _10To16Test()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => AnyRadixConvert._10To16(-1));
            Assert.Equal(AnyRadixConvert._10To16(0), "0");
            Assert.Equal(AnyRadixConvert._10To16(1), "01");
            Assert.Equal(AnyRadixConvert._10To16(10), "0A");
            Assert.Equal(AnyRadixConvert._10To16(100), "064");
        }

        [Fact()]
        public void _16To10Test()
        {
            Assert.Equal(AnyRadixConvert._16To10("0"), 0);
            Assert.Equal(AnyRadixConvert._16To10("01"), 1);
            Assert.Equal(AnyRadixConvert._16To10("A"), 10);
            Assert.Equal(AnyRadixConvert._16To10("64"), 100);
            Assert.Throws<ArgumentException>(() => AnyRadixConvert._16To10("fds&*("));
        }
    }
}
