// -----------------------------------------------------------------------
//  <copyright file="AbstractBuilder.cs" company="OSharp开源团队">
//      Copyright (c) 2014 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2014:07:05 20:52</last-date>
// -----------------------------------------------------------------------

using System.Security.Cryptography;
using System.Text;

using Xunit;


namespace OSharp.Secutiry.Tests
{
    
    public class DesHelperTests
    {
        [Fact()]
        public void DesHelperTest()
        {
            Assert.Equal(8, new DesHelper(false).Key.Length);
            Assert.Equal(24, new DesHelper(true).Key.Length);
        }

        [Fact()]
        public void EncryptTest()
        {
            string key = "12345678";
            string actual = "TMR29YtnGPI=";
            DesHelper des = new DesHelper(Encoding.UTF8.GetBytes(key));
            Assert.Equal(des.Encrypt("admin"), actual);
            Assert.Equal(DesHelper.Encrypt("admin", key), actual);

            //弱密钥
            key = "123456781234567812345678";
            des = new DesHelper(Encoding.UTF8.GetBytes(key));
            Assert.Throws<CryptographicException>(() => des.Encrypt("admin"));

            key = "!@#$%^&*QWERTYUI12345678";
            actual = "Qp4r67VJ8Z0=";
            des = new DesHelper(Encoding.UTF8.GetBytes(key));
            Assert.Equal(des.Encrypt("admin"), actual);
            Assert.Equal(DesHelper.Encrypt("admin", key), actual);
        }

        [Fact()]
        public void DecryptTest()
        {
            string key = "12345678";
            string actual = "TMR29YtnGPI=";
            DesHelper des = new DesHelper(Encoding.UTF8.GetBytes(key));
            Assert.Equal("admin", des.Decrypt(actual));
            Assert.Equal("admin", DesHelper.Decrypt(actual, key));


            key = "!@#$%^&*QWERTYUI12345678";
            actual = "Qp4r67VJ8Z0=";
            des = new DesHelper(Encoding.UTF8.GetBytes(key));
            Assert.Equal("admin", des.Decrypt(actual));
            Assert.Equal("admin", DesHelper.Decrypt(actual, key));
        }

        public void EncryptAndDecryptTest()
        {
            DesHelper des = new DesHelper();
            Assert.Equal("admin", des.Decrypt(des.Encrypt("admin")));
            des = new DesHelper(true);
            Assert.Equal("admin", des.Decrypt(des.Encrypt("admin")));
        }
    }
}