using System;
using System.Collections.Generic;
using System.Linq;

using OSharp.Extensions;
using OSharp.UnitTest.Infrastructure;

using Shouldly;

using Xunit;


namespace OSharp.Security.Tests
{
    public class AesHelperTests
    {
        [Fact]
        public void AesHelperTest()
        {
            string key = new AesHelper().Key;
            Convert.FromBase64String(key).Length.ShouldBe(32);
        }

        [Fact]
        public void Encrypt_Decrypt_Test()
        {
            AesHelper aes = new AesHelper();
            string source = "admin";

            //byte[]
            byte[] sourceBytes = source.ToBytes();
            byte[] enBytes = aes.Encrypt(sourceBytes);
            aes.Decrypt(enBytes).ShouldBe(sourceBytes);

            //string
            string enstr = aes.Encrypt(source);
            aes.Decrypt(enstr).ShouldBe(source);

            aes = new AesHelper(true);

            //byte[]
            enBytes = aes.Encrypt(sourceBytes);
            aes.Decrypt(enBytes).ShouldBe(sourceBytes);

            //string
            enstr = aes.Encrypt(source);
            aes.Decrypt(enstr).ShouldBe(source);
        }

        [Fact]
        public void Encrypt_Decrypt_File_Test()
        {
            string file = @"OSharp.Tests.dll", enFile = "OSharp.Tests_en.dll", deFile = "OSharp.Tests_de.dll";
            AesHelper aes = new AesHelper();
            aes.EncryptFile(file, enFile);
            aes.DecryptFile(enFile, deFile);
            file.ShouldFileMd5Be(deFile);
            enFile.ShouldFileMd5NotBe(deFile);
        }

        [Fact]
        public void VI_NoRepeat_Test()
        {
            List<string> list = new List<string>();
            string source = "admin";
            AesHelper aes = new AesHelper();
            for (int i = 0; i < 100; i++)
            {
                list.Add(aes.Encrypt(source));
            }
            list.Distinct().Count().ShouldBe(1);
            list.Clear();

            aes = new AesHelper(true);
            for (int i = 0; i < 100; i++)
            {
                list.Add(aes.Encrypt(source));
            }
            list.Distinct().Count().ShouldBe(list.Count);
        }
    }
}
