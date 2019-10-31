using System;

using OSharp.Security;

using Shouldly;

using Xunit;


namespace OSharp.Http.Tests
{
    public class TransmissionEncryptorTests
    {
        [Fact]
        public void Trans_Test()
        {
            string source = "client send";

            //客户端加密
            Client client= new Client();
            string enStr = client.Encrypt(source);

            //传输数据。。。
            //传输客户端公钥。。。
            string clientPublicKey = client.PublicKey;
            
            //服务端解密并校验
            Server server = new Server(clientPublicKey);
            string deStr = server.Decrypt(enStr);
            deStr.ShouldBe(source);

            //服务端回复给客户端
            source = "server reply";
            enStr = server.Encrypt(source);
            deStr = client.Decrypt(enStr);
            deStr.ShouldBe(source);
        }

        [Fact]
        public void VerifyError_Test()
        {
            string source = "admin";
            Client client = new Client();
            string enStr = client.Encrypt(source);
            //搞个破坏
            enStr += "A";
            Server server = new Server(client.PublicKey);
            Assert.Throws<FormatException>(() =>
            {
                server.Decrypt(enStr);
            });

            enStr = server.Encrypt(source);
            enStr += "A";
            Assert.Throws<FormatException>(() =>
            {
                client.Decrypt(enStr);
            });
        }

        /// <summary>
        /// 客户端，拥有服务端私钥，自己的公钥和私钥
        /// </summary>
        private class Client
        {
            private const string ServerPublicKey =
                    "<RSAKeyValue><Modulus>rPtaURev4Ztra167DHXTBjrTF7uH8WmD0XePN3JkjYR5u5S+axmuz9/Td90OD3N9IfhwS2uC/aEm/2clv+pUF23yrqIHRFZa0qlWN1FhPr3iXWpKSN0tjBdhdDzXGat6Xfx9MPWlf2dS+B/B+BiaAXnmjsOA6HywIj4gbwHX7DeB68Day9vB61EDwoKbQ9vsVc6DhPnslfmM/SIF7ygYRXQ7VsVOX/N2DS6oHZhVUxlIWk1W0lygUYGJ81qIP3+L386XxhVA+VYxUeX/mu056xH2WA+F8cIYbEEHEsoraDX/nXAyOuBjhK1/MAcShSTOEhtPXMMkU3Myjxsx6u7yIQ==</Modulus><Exponent>AQAB</Exponent><P></P><Q></Q><DP></DP><DQ></DQ><InverseQ></InverseQ><D></D></RSAKeyValue>";
            private readonly TransmissionEncryptor _cryptor;

            public Client()
            {
                RsaHelper rsa = new RsaHelper();
                PublicKey = rsa.PublicKey;
                _cryptor = new TransmissionEncryptor(rsa.PrivateKey, ServerPublicKey);
            }

            public string PublicKey { get; }

            public string Encrypt(string data)
            {
                return _cryptor.EncryptData(data);
            }

            public string Decrypt(string data)
            {
                data = _cryptor.DecryptAndVerifyData(data);
                if (data == null)
                {
                    throw new Exception("数据校验未通过");
                }
                return data;
            }
        }

        /// <summary>
        /// 服务端，拥有服务端私钥，向客户端公开公钥
        /// </summary>
        private class Server
        {
            private const string ServerPrivateKey =
                    "<RSAKeyValue><Modulus>rPtaURev4Ztra167DHXTBjrTF7uH8WmD0XePN3JkjYR5u5S+axmuz9/Td90OD3N9IfhwS2uC/aEm/2clv+pUF23yrqIHRFZa0qlWN1FhPr3iXWpKSN0tjBdhdDzXGat6Xfx9MPWlf2dS+B/B+BiaAXnmjsOA6HywIj4gbwHX7DeB68Day9vB61EDwoKbQ9vsVc6DhPnslfmM/SIF7ygYRXQ7VsVOX/N2DS6oHZhVUxlIWk1W0lygUYGJ81qIP3+L386XxhVA+VYxUeX/mu056xH2WA+F8cIYbEEHEsoraDX/nXAyOuBjhK1/MAcShSTOEhtPXMMkU3Myjxsx6u7yIQ==</Modulus><Exponent>AQAB</Exponent><P>8G7R0LQ80eydXggU0ctZO458UGlhkvYxtZ3ZWaPYxBrHk9WCN2PaSCfHikSn9XjWVNZFOH3Sx4GyWvI5X72p3Pb4cnXXb76/y9GkmcFSXAq4D38Lx6r4i58L8X6EhVcAXe3op01yeBtBN1p+wuzZf1rhWw3vK78seFjXTLFtg3c=</P><Q>uC6H3FgeJqG4z1H45pozjPEBt6roimOB9Zw+v+tVJvYWQ27vn+5QX3baM4ItIgzG8d9JVRhXOINlr+TqF/NgSrVArHR32gldmob87nJ2G+om2Y8L+d6eSy8fjwNrfyCkP3SNynrYrribyDTa4MUUJkjPOoPNQ0PpFMxzzxb8LSc=</Q><DP>YSkneOOdQGgSIBG7+Bvo73xhGE29tJnw9KfZUbQ4wObhiAhcGu4rI8WPiXy9MyGl25rLVkzihOUQolgIf3wxzK2xMPAWI34+G6uYjNnm4nMidoCszf91eVbIiMrL2uaRq2OdBR7zBz5cWHYli7gTHLgpIZa79D3JRskAGGoLC3k=</DP><DQ>M4LH+ocmf5VxU4JQg2YTDtMEsPJ6sOGdoix9nD1cBlaC6X8oQ2lqTxi1c/xvVPuP1GunXcY99o9BLE1wbxxhDOPeX5z/PBqsdCBWsvWONMZq8SPBrLNnQA3A3MWPFfHyHdEiKsPqwArffsHRiVV1CjQIyQu9p5dho7B+nyeFhbc=</DQ><InverseQ>0SD3w139vW5xHInnm9phBuxrOBwpukwgnRCOZ2ETlLiBbVqkWh7jlY6IZUgIIKUOSq6Jxn+iFmdtIzX8yR6EFzDouc9dLZnACBNLKnk1/137KByZMDn4hSbrClgIMK7dSfVd3+tk1JSkk7eQOOC2MCgwqT0fnKb8mLoKjTlze6Q=</InverseQ><D>E1QmvGb0LTZro1y7c+H6iXirO7ylrREIPLCqXRy4JaQG4jH1sQv2n524CG2383wJIHGdQqApy+Nk8yb4beBmMxQDCQZr33PyxBVN3/KE9jjLgXquAEROoZe/OPjPZDvn+XaMGptoObs25yjNSMae9slDPPKVheTg3YKppx3Cnc9Kj9sxnnNqjKwupw9ghky7iWgG0wmiE28Pkfaa5gKTaIvhdLbE19J3Vp0CVl2Mm1he6qSy4boadU9Yd7OjcvelEaqDDbOqMmLRM1c2LF1MGxGLS5eW57jlpR9soOTvcvQb0NE1cthAJtW1XSPj/U/tgaEFT61ZNzivkgpBXuwa7Q==</D></RSAKeyValue>";
            private readonly TransmissionEncryptor _cryptor;

            public Server(string clientPublicKey)
            {
                _cryptor = new TransmissionEncryptor(ServerPrivateKey, clientPublicKey);
            }

            public string Decrypt(string data)
            {
                data = _cryptor.DecryptAndVerifyData(data);
                if (data == null)
                {
                    throw new Exception("数据校验未通过");
                }
                return data;
            }

            public string Encrypt(string data)
            {
                return _cryptor.EncryptData(data);
            }
        }
    }

    
}
