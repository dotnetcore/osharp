using Xunit;
namespace OSharp.Security.Tests
{
    
    public class HashHelperTests
    {
        [Fact()]
        public void GetMd5Test()
        {
            const string value = "admin";
            const string actual = "21232f297a57a5a743894a0e4a801fc3";
            Assert.Equal(HashHelper.GetMd5(value), actual);
        }

        [Fact()]
        public void GetSha1Test()
        {
            const string value = "admin";
            const string actual = "d033e22ae348aeb5660fc2140aec35850c4da997";
            Assert.Equal(HashHelper.GetSha1(value), actual);
        }

        [Fact()]
        public void GetSha256Test()
        {
            const string value = "admin";
            const string actual = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918";
            Assert.Equal(HashHelper.GetSha256(value), actual);
        }

        [Fact()]
        public void GetSha512Test()
        {
            const string value = "admin";
            const string actual = "c7ad44cbad762a5da0a452f9e854fdc1e0e7a52a38015f23f3eab1d80b931dd472634" +
                "dfac71cd34ebc35d16ab7fb8a90c81f975113d6c7538dc69dd8de9077ec";
            Assert.Equal(HashHelper.GetSha512(value), actual);
        }
    }
}
