using OSharp.IO;

using Xunit;


namespace OSharp.UnitTest.Infrastructure
{
    public static class AssertExtensions
    {
        /// <summary>
        /// 断言两个文件的MD5一致
        /// </summary>
        public static void ShouldFileMd5Be(this string sourceFile, string targetFile)
        {
            string md51 = FileHelper.GetFileMd5(sourceFile);
            string md52 = FileHelper.GetFileMd5(targetFile);
            Assert.Equal(md51,md52);
        }

        /// <summary>
        /// 断言两个文件的MD5不一致
        /// </summary>
        public static void ShouldFileMd5NotBe(this string sourceFile, string targetFile)
        {
            string md51 = FileHelper.GetFileMd5(sourceFile);
            string md52 = FileHelper.GetFileMd5(targetFile);
            Assert.NotEqual(md51, md52);
        }
    }
}
