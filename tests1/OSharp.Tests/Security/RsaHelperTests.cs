// -----------------------------------------------------------------------
//  <copyright file="RsaHelperTests.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-12-30 22:12</last-date>
// -----------------------------------------------------------------------

using OSharp.Extensions;
using OSharp.Security;

using Shouldly;

using Xunit;


namespace OSharp.Tests.Security
{
    public class RsaHelperTests
    {
        [Fact]
        public void Encrypt_Decrypt_Test()
        {
            string source = "admin";
            RsaHelper rsa = new RsaHelper();

            //byte[]
            byte[] sourceBytes = source.ToBytes();
            byte[] enBytes = rsa.Encrypt(sourceBytes);
            rsa.Decrypt(enBytes).ShouldBe(sourceBytes);

            //string
            string enstr = rsa.Encrypt(source);
            rsa.Decrypt(enstr).ShouldBe(source);
        }

        [Fact]
        public void RsaHelperTest()
        {
            RsaHelper rsa = new RsaHelper();
            rsa.PrivateKey.Contains(rsa.PublicKey.Substring("<Modulus>", "</Modulus>")).ShouldBeTrue();
        }

        [Fact]
        public void Sign_Test()
        {
            string source = "admin";
            RsaHelper rsa = new RsaHelper();

            byte[] sourceBytes = source.ToBytes();
            byte[] signData = rsa.SignData(sourceBytes);
            rsa.VerifyData(sourceBytes, signData).ShouldBeTrue();

            string signStr = rsa.SignData(source);
            rsa.VerifyData(source, signStr).ShouldBeTrue();
        }
    }
}