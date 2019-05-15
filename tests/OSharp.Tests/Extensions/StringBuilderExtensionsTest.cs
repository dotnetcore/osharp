using System.Text;

using OSharp.Extensions;

using Xunit;


namespace OSharp.Tests.Extensions
{
    
    public class StringBuilderExtensionsTest
    {
        [Fact()]
        public void TrimTest()
        {
            StringBuilder sb = null;
            sb = new StringBuilder("   asd sdf  ");
            Assert.Equal("asd sdf", sb.Trim().ToString());
        }

        [Fact()]
        public void TrimStartTest()
        {
            StringBuilder sb = new StringBuilder("asdfgef");
            Assert.Equal("sdfgef", sb.TrimStart('a').ToString());
            sb.Insert(0, "   ");
            Assert.Equal("sdfgef", sb.TrimStart().ToString());
            Assert.Equal("gef", sb.TrimStart("sdf").ToString());
            Assert.Equal(sb.TrimStart("gef").ToString(), string.Empty);
        }

        [Fact()]
        public void TrimEndTest()
        {
            StringBuilder sb;
            sb = new StringBuilder("asdfgef");
            Assert.Equal("asdfgef", sb.TrimEnd('a').ToString());
            Assert.Equal("asdfge", sb.TrimEnd('f').ToString());
            sb.Append("   ");
            Assert.Equal("asdfge", sb.TrimEnd().ToString());
            Assert.Equal("asdf", sb.TrimEnd(new[] { 'g', 'e' }).ToString());
            Assert.Equal(sb.TrimEnd("asdf").ToString(), string.Empty);
        }
    }
}
