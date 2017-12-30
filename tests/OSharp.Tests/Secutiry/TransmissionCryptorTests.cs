using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

using Xunit;


namespace OSharp.Secutiry.Tests
{
    public class TransmissionCryptorTests
    {
        [Fact]
        public void Test()
        {
            RsaHelper rsa = new RsaHelper();
            string puk = rsa.PublicKey;
            string pvk = rsa.PrivateKey;
        }

        /// <summary>
        /// 客户端，拥有服务端私钥，自己的公钥和私钥
        /// </summary>
        private class Client
        {
            
        }

        /// <summary>
        /// 服务端，拥有服务端私钥，向客户端公开公钥
        /// </summary>
        private class Server
        {
            
        }
    }

    
}
