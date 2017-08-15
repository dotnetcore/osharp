using System;
using System.Linq;
using System.Text;

using Xunit;

namespace OSharp.Extensions.Tests
{
    public class StringExtensionsTests
    {
        [Fact()]
        public void IsMatchTest()
        {
            const string pattern = @"\d.*";
            Assert.False(((string)null).IsMatch(pattern));
            Assert.False("abc".IsMatch(pattern));
            Assert.True("abc123".IsMatch(pattern));
        }

        [Fact()]
        public void MatchTest()
        {
            const string pattern = @"\d.*";
            Assert.Null(((string)null).Match(pattern));
            Assert.Equal("abc".Match(pattern), string.Empty);
            Assert.Equal("abc123".Match(pattern), "123");
        }

        [Fact()]
        public void MatchesTest()
        {
            const string pattern = @"\d";
            Assert.Equal(((string)null).Matches(pattern).Count(), 0);
            Assert.Equal("abc".Matches(pattern).Count(), 0);
            Assert.Equal("abc123".Matches(pattern).Count(), 3);
        }

        [Fact()]
        public void StrLengthTest()
        {
            Assert.Equal("".TextLength(), 0);
            Assert.Equal("123".TextLength(), 3);
            Assert.Equal("abc".TextLength(), 3);
            Assert.Equal("$%^*&".TextLength(), 5);
            Assert.Equal("汉字测试".TextLength(), 8);
        }

        [Fact()]
        public void IsEmailTest()
        {
            string value = null;
            Assert.False(value.IsEmail());
            value = "123";
            Assert.False(value.IsEmail());
            value = "abc123.fds";
            Assert.False(value.IsEmail());
            value = "abc.yeah.net";
            Assert.False(value.IsEmail());
            value = "abc@yeah.net";
            Assert.True(value.IsEmail());
            value = "abc.a@yeah.net";
            Assert.True(value.IsEmail());
        }

        [Fact()]
        public void IsIpAddressTest()
        {
            string value = null;
            Assert.False(value.IsIpAddress());
            value = "321.ad.54.22";
            Assert.False(value.IsIpAddress());
            value = "0.0.0.0";
            Assert.True(value.IsIpAddress());
            value = "1.1.1.1";
            Assert.True(value.IsIpAddress());
            value = "192.168.0.1";
            Assert.True(value.IsIpAddress());
            value = "255.255.255.255";
            Assert.True(value.IsIpAddress());
        }

        [Fact()]
        public void AddUrlQueryTest()
        {
            const string url = "http://localhost:801";
            string excepted = url + "?id=1";
            Assert.Equal(url.AddUrlQuery("id=1"), excepted);
            excepted = url + "?name=abc";
            Assert.Equal(url.AddUrlQuery("name=abc"), excepted);
            excepted = url + "?id=1&name=abc";
            Assert.Equal(url.AddUrlQuery("id=1", "name=abc"), excepted);
        }

        [Fact()]
        public void GetQueryParamTest()
        {
            string url = "http://www.baidu.com?key=website&word=beyond&name=%E9%83%AD%E6%98%8E%E9%94%8B";
            Assert.Equal(url.GetUrlQuery("key"), "website");
            Assert.Equal(url.GetUrlQuery("word"), "beyond");
            Assert.Equal(url.GetUrlQuery("name"), "%E9%83%AD%E6%98%8E%E9%94%8B");
            Assert.Equal(url.GetUrlQuery("nokey"), string.Empty);
        }

        [Fact()]
        public void AddHashFragmentTest()
        {
            const string url = "http://localhost:801";
            string excepted = url + "#title";
            Assert.Equal(url.AddHashFragment("title"), excepted);
        }

        [Fact()]
        public void MatchFirstNumberTest()
        {
            const string source = "电话号码：13800138000，卡号：123456789，QQ号码：123202901，记住了吗？";
            Assert.Equal(source.MatchFirstNumber(), "13800138000");
        }

        [Fact()]
        public void MatchLastNumberTest()
        {
            const string source = "电话号码：13800138000，卡号：123456789，QQ号码：123202901，记住了吗？";
            Assert.Equal(source.MatchLastNumber(), "123202901");
        }

        [Fact()]
        public void MatchNumbersTest()
        {
            const string source = "电话号码：13800138000，卡号：123456789，QQ号码：123202901，记住了吗？";
            Assert.Equal(source.MatchNumbers(), new[] { "13800138000", "123456789", "123202901" });
        }

        [Fact()]
        public void IsMatchNumberTest()
        {
            string source = "电话号码：13800138000，卡号：123456789，QQ号码：123202901，记住了吗？";
            Assert.True(source.IsMatchNumber());
            source = "你以为你委膙啊";
            Assert.False(source.IsMatchNumber());
        }

        [Fact()]
        public void IsMatchNumberTest1()
        {
            string source = "123456789";
            Assert.True(source.IsMatchNumber(9));
            source = "123456789s";
            Assert.False(source.IsMatchNumber(9));
        }

        [Fact()]
        public void SubstringTest()
        {
            const string source = "http://vote3.52meirongwang.com/members/vote_detail.aspx?id=484&pid=37857&from=groupmessage&isappinstalled=0";
            Assert.Equal(source.Substring("?id=", "&"), "484");
            Assert.Equal(source.Substring("&pid=", "&"), "37857");
            Assert.Equal(source.Substring("&isappinstalled=", "&", ""), "0");
        }

        [Fact()]
        public void IsUnicodeTest()
        {
            string source = "今天天气不错";
            Assert.True(source.IsUnicode());
            source = "abc123";
            Assert.False(source.IsUnicode());
        }

        [Fact()]
        public void IsIdentityCardTest()
        {
            string value = "321081199801018994";
            Assert.True(value.IsIdentityCardId());
            value = "371328198104016829";
            Assert.True(value.IsIdentityCardId());
            value = "37132819810401652x";
            Assert.True(value.IsIdentityCardId());
        }

        [Fact()]
        public void GetChineseSpellTest()
        {
            char @char = '郭';
            Assert.Equal(@char.GetChineseSpell(), "G");

            string str = "郭明锋";
            Assert.Equal(str.GetChineseSpell(), "GMF");
        }

        [Fact()]
        public void ToUnicodeStringTest()
        {
            string unicode = "编码".ToUnicodeString();
            Assert.Equal(@"\u7f16\u7801", unicode);
            Assert.Equal("编码", unicode.FromUnicodeString());
        }

        [Fact()]
        public void ToHexStringTest()
        {
            string str1 = "http://b1.1ydb360.com/app/index.php?i=8&c=entry&rid=53&id=2286&do=view&m=tyzm_diamondvote&wxref=mp.weixin.qq.com&from=groupmessage&winzoom=1";
            string hex = str1.ToHexString();
            string str2 = hex.FromHexString();
            Assert.Equal(str1, str2);
        }

        [Fact()]
        public void IsUrlTest()
        {
            string url =
                "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx2cb8ad06a252b27c&redirect_uri=http%3A%2F%2Ftpkwx.tpk.com%2FAdmin%2FWeixin%2FWeixinAuthCallback.aspx&response_type=code&scope=snsapi_userinfo&state=1$promotion#wechat_redirect";
            Assert.True(url.IsUrl());
        }
    }
}
