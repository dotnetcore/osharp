using Xunit;
using System;
using System.Linq;


namespace OSharp.Extensions.Tests
{
    
    public class RandomExtensionsTest
    {
        [Fact()]
        public void NextBooleanTest()
        {
            Random rnd = new Random();
            bool value = rnd.NextBoolean();
            Assert.Contains(value, new[] { true, false });
        }

        [Fact()]
        public void NextEnumTest()
        {
            Random rnd = new Random();
            UriKind kind = rnd.NextEnum<UriKind>();
            Assert.Contains(kind, new[] { UriKind.Absolute, UriKind.Relative, UriKind.RelativeOrAbsolute });
        }

        [Fact()]
        public void NextBytesTest()
        {
            Random rnd = new Random();
            Assert.Throws<ArgumentOutOfRangeException>(() => rnd.NextBytes(-5));

            byte[] bytes = rnd.NextBytes(10);
            Assert.True(bytes.Length == 10);
            Assert.True(bytes.Distinct().Count() > 5);
        }

        [Fact()]
        public void NextItemTest()
        {
            Random rnd = new Random();
            int[] array = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int item = rnd.NextItem(array);
            Assert.Contains(item, array);
        }

        [Fact()]
        public void NextDateTimeTest()
        {
            Random rnd = new Random();
            DateTime dt = rnd.NextDateTime();
            Assert.True(dt >= DateTime.MinValue);
            Assert.True(dt <= DateTime.MaxValue);

            DateTime dtNow = DateTime.Now;
            DateTime dtMin = dtNow.AddMinutes(-10);
            DateTime dtMax = dtNow.AddMinutes(10);
            dt = rnd.NextDateTime(dtMin, dtMax);
            Assert.True(dt >= dtMin);
            Assert.True(dt <= dtMax);
        }

        [Fact()]
        public void GetRandomNumberStringTest()
        {
            Random rnd = new Random();
            Assert.Throws<ArgumentOutOfRangeException>(() => rnd.NextNumberString(-5));
            string rndNum = rnd.NextNumberString(10);
            Assert.True(rndNum.Length == 10);
        }

        [Fact()]
        public void GetRandomLetterStringTest()
        {
            Random rnd = new Random();
            Assert.Throws<ArgumentOutOfRangeException>(() => rnd.NextNumberString(-5));
            string rndNum = rnd.NextLetterString(10);
            Assert.True(rndNum.Length == 10);
        }

        [Fact()]
        public void GetRandomLetterAndNumberString()
        {
            Random rnd = new Random();
            Assert.Throws<ArgumentOutOfRangeException>(() => rnd.NextNumberString(-5));
            string rndNum = rnd.NextLetterAndNumberString(10);
            Assert.True(rndNum.Length == 10);
        }
    }
}
