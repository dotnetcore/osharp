using Xunit;

namespace OSharp.Data.Tests
{
    public class PinYinConverterTests
    {
        [Fact()]
        public void GetFirstTest()
        {
            string input = "把汉字转换成拼音";
            string actual = "BHZZHCPY";
            Assert.Equal(PinYinConverter.GetFirst(input), actual);
            input = "把汉字转换成拼音";
            actual = "BaHanZiZhuanHuanChengPinYin";
            Assert.Equal(PinYinConverter.Get(input), actual);
            input = "把汉字，转换,成<拼音>";
            actual = "BaHanZi，ZhuanHuan,Cheng<PinYin>";
            Assert.Equal(PinYinConverter.Get(input), actual);
        }
    }
}