using OSharp.Collections;

using Shouldly;

using Xunit;


namespace OSharp.Caching.Tests
{
    public class ArrayExtensionsTests
    {
        [Fact]
        public void CopyTest()
        {
            byte[,] bytes = new byte[2, 3] { { 10, 11, 12 }, { 13, 14, 15 } };
            byte[,] newBytes = bytes.Copy();
            newBytes[0, 1].ShouldBe(bytes[0, 1]);
            newBytes.ShouldBe(bytes);
        }
    }
}
