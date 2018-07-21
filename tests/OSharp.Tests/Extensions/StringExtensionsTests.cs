using System.Linq;

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
            Assert.Equal("123", "abc123".Match(pattern));
        }

        [Fact()]
        public void MatchesTest()
        {
            const string pattern = @"\d";
            Assert.Empty(((string)null).Matches(pattern));
            Assert.Empty("abc".Matches(pattern));
            Assert.Equal(3, "abc123".Matches(pattern).Count());
        }

        [Fact()]
        public void StrLengthTest()
        {
            Assert.Equal(0, "".TextLength());
            Assert.Equal(3, "123".TextLength());
            Assert.Equal(3, "abc".TextLength());
            Assert.Equal(5, "$%^*&".TextLength());
            Assert.Equal(8, "汉字测试".TextLength());
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
            Assert.Equal("website", url.GetUrlQuery("key"));
            Assert.Equal("beyond", url.GetUrlQuery("word"));
            Assert.Equal("%E9%83%AD%E6%98%8E%E9%94%8B", url.GetUrlQuery("name"));
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
            Assert.Equal("13800138000", source.MatchFirstNumber());
        }

        [Fact()]
        public void MatchLastNumberTest()
        {
            const string source = "电话号码：13800138000，卡号：123456789，QQ号码：123202901，记住了吗？";
            Assert.Equal("123202901", source.MatchLastNumber());
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
            Assert.Equal("484", source.Substring("?id=", "&"));
            Assert.Equal("37857", source.Substring("&pid=", "&"));
            Assert.Equal("0", source.Substring("&isappinstalled=", "&", ""));
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
            value = "37132819810401653x";
            Assert.False(value.IsIdentityCardId());
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
