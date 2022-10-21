// -----------------------------------------------------------------------
//  <copyright file="IpLocator.cs" company="OSharp开源团队">
//      Copyright (c) 2014 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2014-10-14 1:28</last-date>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;


namespace OSharp.Net
{
    /// <summary>
    /// IP位置查找操作类
    /// </summary>
    public class IpLocator
    {
        private readonly byte[] _data;
        private readonly long _firstStartIpOffset;
        private static readonly Regex Regex = new Regex(@"^(((\d{1,2})|(1\d{2})|(2[0-4]\d)|(25[0-5]))\.){3}((\d{1,2})|(1\d{2})|(2[0-4]\d)|(25[0-5]))$");

        /// <summary>
        /// 初始化一个IP位置查找操作类的实例
        /// </summary>
        /// <param name="dataPath"> IP信息数据文件路径 </param>
        /// <exception cref="ArgumentException"></exception>
        public IpLocator(string dataPath)
        {
            using (FileStream fs = new FileStream(dataPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                _data = new byte[fs.Length];
                fs.Read(_data, 0, _data.Length);
            }
            byte[] buffer = new byte[8];
            Array.Copy(_data, 0, buffer, 0, 8);
            _firstStartIpOffset = ((buffer[0] + (buffer[1] * 0x100)) + ((buffer[2] * 0x100) * 0x100)) + (((buffer[3] * 0x100) * 0x100) * 0x100);
            long lastStartIpOffset = ((buffer[4] + (buffer[5] * 0x100)) + ((buffer[6] * 0x100) * 0x100)) + (((buffer[7] * 0x100) * 0x100) * 0x100);
            Count = Convert.ToInt64((lastStartIpOffset - _firstStartIpOffset) / 7.0);

            if (Count <= 1L)
            {
                throw new ArgumentException("IP信息数据文件异常。");
            }
        }

        /// <summary>
        /// 数据文件中信息数量
        /// </summary>
        public long Count { get; private set; }

        /// <summary>
        /// IP地址转化成整数
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static long IpToInt(string ip)
        {
            char[] separator = new[] { '.' };
            if (ip.Split(separator).Length == 3)
            {
                ip = ip + ".0";
            }

            if (!Regex.Match(ip).Success)
            {
                throw new ArgumentException("IP格式错误");
            }
            string[] nums = ip.Split(separator);
            long num1 = ((long.Parse(nums[0]) * 0x100L) * 0x100L) * 0x100L;
            long num2 = (long.Parse(nums[1]) * 0x100L) * 0x100L;
            long num3 = long.Parse(nums[2]) * 0x100L;
            long num4 = long.Parse(nums[3]);
            return (((num1 + num2) + num3) + num4);
        }

        /// <summary>
        /// IP地址从整数类型转化为正常IP类型
        /// </summary>
        /// <param name="ipInt"></param>
        /// <returns></returns>
        public static string IntToIp(long ipInt)
        {
            long num1 = (ipInt & 0xff000000L) >> 0x18;
            if (num1 < 0L)
            {
                num1 += 0x100L;
            }
            long num2 = (ipInt & 0xff0000L) >> 0x10;
            if (num2 < 0L)
            {
                num2 += 0x100L;
            }
            long num3 = (ipInt & 0xff00L) >> 8;
            if (num3 < 0L)
            {
                num3 += 0x100L;
            }
            long num4 = ipInt & 0xffL;
            if (num4 < 0L)
            {
                num4 += 0x100L;
            }
            string ip = string.Concat(new[]
            {
                num1.ToString(CultureInfo.InvariantCulture),
                ".",
                num2.ToString(CultureInfo.InvariantCulture),
                ".",
                num3.ToString(CultureInfo.InvariantCulture),
                ".",
                num4.ToString(CultureInfo.InvariantCulture)
            });

            if (!Regex.Match(ip).Success)
            {
                throw new ArgumentException("IP格式错误");
            }
            return ip;
        }

        /// <summary>
        /// 由IP地址查找对应的位置信息
        /// </summary>
        /// <param name="ip"> 要查找的IP地址 </param>
        /// <returns> </returns>
        /// <exception cref="ArgumentException"></exception>
        public IpLocation Query(string ip)
        {
            if (ip == "::1")
            {
                ip = "127.0.0.1";
            }
            if (!Regex.Match(ip).Success)
            {
                throw new ArgumentException("IP格式错误");
            }
            IpLocation ipLocation = new IpLocation { Ip = ip };
            long intIp = IpToInt(ip);
            if ((intIp >= IpToInt("127.0.0.1") && (intIp <= IpToInt("127.255.255.255"))))
            {
                ipLocation.Country = "本机内部环回地址";
                ipLocation.Local = "";
            }
            else
            {
                if ((((intIp >= IpToInt("0.0.0.0")) && (intIp <= IpToInt("2.255.255.255"))) ||
                    ((intIp >= IpToInt("64.0.0.0")) && (intIp <= IpToInt("126.255.255.255")))) ||
                    ((intIp >= IpToInt("58.0.0.0")) && (intIp <= IpToInt("60.255.255.255"))))
                {
                    ipLocation.Country = "网络保留地址";
                    ipLocation.Local = "";
                }
            }
            long right = Count;
            long left = 0L;
            long startIp;
            long endIpOff;
            int countryFlag;
            while (left < (right - 1L))
            {
                long middle = (right + left) / 2L;
                startIp = GetStartIp(middle, out endIpOff);
                if (intIp == startIp)
                {
                    left = middle;
                    break;
                }
                if (intIp > startIp)
                {
                    left = middle;
                }
                else
                {
                    right = middle;
                }
            }
            startIp = GetStartIp(left, out endIpOff);
            long endIp = GetEndIp(endIpOff, out countryFlag);
            if ((startIp <= intIp) && (endIp >= intIp))
            {
                string local;
                ipLocation.Country = GetCountry(endIpOff, countryFlag, out local);
                ipLocation.Local = local.Replace("（我们一定要解放台湾！！！）", "");
            }
            else
            {
                ipLocation.Country = "未知";
                ipLocation.Local = "";
            }
            return ipLocation;
        }

        /// <summary>
        /// 由IP地址查找对应的位置信息的字符串
        /// </summary>
        public string Query2(string ip)
        {
            IpLocation result = Query(ip);
            return (result.Country + result.Local).Replace("CZ88.NET", "");
        }

        private long GetStartIp(long left, out long endIpOff)
        {
            long leftOffset = _firstStartIpOffset + (left * 7L);
            byte[] buffer = new byte[7];
            Array.Copy(_data, leftOffset, buffer, 0, 7);
            endIpOff = (Convert.ToInt64(buffer[4].ToString(CultureInfo.InvariantCulture)) +
                (Convert.ToInt64(buffer[5].ToString(CultureInfo.InvariantCulture)) * 0x100L)) +
                ((Convert.ToInt64(buffer[6].ToString(CultureInfo.InvariantCulture)) * 0x100L) * 0x100L);
            return ((Convert.ToInt64(buffer[0].ToString(CultureInfo.InvariantCulture)) +
                (Convert.ToInt64(buffer[1].ToString(CultureInfo.InvariantCulture)) * 0x100L)) +
                ((Convert.ToInt64(buffer[2].ToString(CultureInfo.InvariantCulture)) * 0x100L) * 0x100L)) +
                (((Convert.ToInt64(buffer[3].ToString(CultureInfo.InvariantCulture)) * 0x100L) * 0x100L) * 0x100L);
        }

        private long GetEndIp(long endIpOff, out int countryFlag)
        {
            byte[] buffer = new byte[5];
            Array.Copy(_data, endIpOff, buffer, 0, 5);
            countryFlag = buffer[4];
            return ((Convert.ToInt64(buffer[0].ToString(CultureInfo.InvariantCulture)) +
                (Convert.ToInt64(buffer[1].ToString(CultureInfo.InvariantCulture)) * 0x100L)) +
                ((Convert.ToInt64(buffer[2].ToString(CultureInfo.InvariantCulture)) * 0x100L) * 0x100L)) +
                (((Convert.ToInt64(buffer[3].ToString(CultureInfo.InvariantCulture)) * 0x100L) * 0x100L) * 0x100L);
        }

        /// <summary>
        /// Gets the country.
        /// </summary>
        /// <param name="endIpOff"> The end ip off. </param>
        /// <param name="countryFlag"> The country flag. </param>
        /// <param name="local"> The local. </param>
        /// <returns> country </returns>
        private string GetCountry(long endIpOff, int countryFlag, out string local)
        {
            string country;
            long offset = endIpOff + 4L;
            switch (countryFlag)
            {
                case 1:
                case 2:
                    country = GetFlagStr(ref offset, ref countryFlag, ref endIpOff);
                    offset = endIpOff + 8L;
                    local = (1 == countryFlag) ? "" : GetFlagStr(ref offset, ref countryFlag, ref endIpOff);
                    break;
                default:
                    country = GetFlagStr(ref offset, ref countryFlag, ref endIpOff);
                    local = GetFlagStr(ref offset, ref countryFlag, ref endIpOff);
                    break;
            }
            return country;
        }

        private string GetFlagStr(ref long offset, ref int countryFlag, ref long endIpOff)
        {
            byte[] buffer = new byte[3];

            while (true)
            {
                //用于向前累加偏移量
                long forwardOffset = offset;
                int flag = _data[forwardOffset++];
                //没有重定向
                if (flag != 1 && flag != 2)
                {
                    break;
                }
                Array.Copy(_data, forwardOffset, buffer, 0, 3);
                if (flag == 2)
                {
                    countryFlag = 2;
                    endIpOff = offset - 4L;
                }
                offset = (Convert.ToInt64(buffer[0].ToString()) + (Convert.ToInt64(buffer[1].ToString()) * 0x100L)) +
                    ((Convert.ToInt64(buffer[2].ToString()) * 0x100L) * 0x100L);
            }
            if (offset < 12L)
            {
                return "";
            }
            return GetStr(ref offset);
        }

        private string GetStr(ref long offset)
        {
            StringBuilder stringBuilder = new StringBuilder();
            byte[] bytes = new byte[2];
            Encoding encoding = Encoding.GetEncoding("GB2312");
            while (true)
            {
                byte lowByte = _data[offset++];
                if (lowByte == 0)
                {
                    return stringBuilder.ToString();
                }
                if (lowByte > 0x7f)
                {
                    byte highByte = _data[offset++];
                    bytes[0] = lowByte;
                    bytes[1] = highByte;
                    if (highByte == 0)
                    {
                        return stringBuilder.ToString();
                    }
                    stringBuilder.Append(encoding.GetString(bytes));
                }
                else
                {
                    stringBuilder.Append((char)lowByte);
                }
            }
        }
    }
}