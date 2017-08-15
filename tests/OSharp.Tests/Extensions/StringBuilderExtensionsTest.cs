using System.Text;

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
            Assert.Equal(sb.Trim().ToString(), "asd sdf");
        }

        [Fact()]
        public void TrimStartTest()
        {
            StringBuilder sb = new StringBuilder("asdfgef");
            Assert.Equal(sb.TrimStart('a').ToString(), "sdfgef");
            sb.Insert(0, "   ");
            Assert.Equal(sb.TrimStart().ToString(), "sdfgef");
            Assert.Equal(sb.TrimStart("sdf").ToString(), "gef");
            Assert.Equal(sb.TrimStart("gef").ToString(), string.Empty);
        }

        [Fact()]
        public void TriemEndTest()
        {
            StringBuilder sb;
            sb = new StringBuilder("asdfgef");
            Assert.Equal(sb.TrimEnd('a').ToString(), "asdfgef");
            Assert.Equal(sb.TrimEnd('f').ToString(), "asdfge");
            sb.Append("   ");
            Assert.Equal(sb.TrimEnd().ToString(), "asdfge");
            Assert.Equal(sb.TrimEnd(new[] { 'g', 'e' }).ToString(), "asdf");
            Assert.Equal(sb.TrimEnd("asdf").ToString(), string.Empty);
        }
    }
}
