using System;

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
            Assert.Equal(0UL, AnyRadixConvert.X2H("0", 2));
            Assert.Equal(2UL, AnyRadixConvert.X2H("10", 2));
            Assert.Equal(10UL, AnyRadixConvert.X2H("10", 10));
            Assert.Equal(10UL, AnyRadixConvert.X2H("A", 16));
        }

        [Fact()]
        public void H2XTest()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => AnyRadixConvert.H2X(123UL, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => AnyRadixConvert.H2X(123UL, 63));
            Assert.Equal("101", AnyRadixConvert.H2X(5UL, 2));
            Assert.Equal("0", AnyRadixConvert.H2X(0, 2));
            Assert.Equal("10", AnyRadixConvert.H2X(2, 2));
            Assert.Equal("10", AnyRadixConvert.H2X(10, 10));
            Assert.Equal("A", AnyRadixConvert.H2X(10, 16));
        }

        [Fact()]
        public void X2XTest()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => AnyRadixConvert.X2X("0", 0, 10));
            Assert.Throws<ArgumentOutOfRangeException>(() => AnyRadixConvert.X2X("0", 10, 63));
            Assert.Throws<ArgumentException>(() => AnyRadixConvert.X2X("2", 2, 10));
            Assert.Throws<ArgumentException>(() => AnyRadixConvert.X2X("A", 10, 10));
            Assert.Throws<ArgumentException>(() => AnyRadixConvert.X2X("G", 16, 10));
            Assert.Equal("0", AnyRadixConvert.X2X("0", 2, 10));
            Assert.Equal("4", AnyRadixConvert.X2X("100", 2, 10));
            Assert.Equal("64", AnyRadixConvert.X2X("100", 10, 16));
            Assert.Equal("256", AnyRadixConvert.X2X("100", 16, 10));
        }

        [Fact()]
        public void _10To16Test()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => AnyRadixConvert._10To16(-1));
            Assert.Equal("0", AnyRadixConvert._10To16(0));
            Assert.Equal("01", AnyRadixConvert._10To16(1));
            Assert.Equal("0A", AnyRadixConvert._10To16(10));
            Assert.Equal("064", AnyRadixConvert._10To16(100));
        }

        [Fact()]
        public void _16To10Test()
        {
            Assert.Equal(0, AnyRadixConvert._16To10("0"));
            Assert.Equal(1, AnyRadixConvert._16To10("01"));
            Assert.Equal(10, AnyRadixConvert._16To10("A"));
            Assert.Equal(100, AnyRadixConvert._16To10("64"));
            Assert.Throws<ArgumentException>(() => AnyRadixConvert._16To10("fds&*("));
        }
    }
}
