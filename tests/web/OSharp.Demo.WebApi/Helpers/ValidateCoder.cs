// -----------------------------------------------------------------------
//  <copyright file="ValidateCoder.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-05-04 21:01</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using OSharp.Collections;
using OSharp.Data;


namespace OSharp.Demo.WebApi.Helpers
{
    /// <summary>
    /// 验证码生成类
    /// </summary>
    public class ValidateCoder
    {
        private static readonly Random Random = new Random();

        /// <summary>
        /// 初始化一个<see cref="ValidateCoder"/>类型的新实例
        /// </summary>
        public ValidateCoder()
        {
            FontNames = new List<string> { "Arial", "Batang", "Buxton Sketch", "David", "SketchFlow Print" };
            FontNamesForHanzi = new List<string> { "宋体", "幼圆", "楷体", "仿宋", "隶书", "黑体" };
            FontSize = 20;
            FontWidth = FontSize;
            BgColor = Color.FromArgb(240, 240, 240);
            RandomPointPercent = 0;
        }

        #region 属性

        /// <summary>
        /// 获取或设置 字体名称集合
        /// </summary>
        public List<string> FontNames { get; set; }

        /// <summary>
        /// 获取或设置 汉字字体名称集合
        /// </summary>
        public List<string> FontNamesForHanzi { get; set; }

        /// <summary>
        /// 获取或设置 字体大小
        /// </summary>
        public int FontSize { get; set; }

        /// <summary>
        /// 获取或设置 字体宽度
        /// </summary>
        public int FontWidth { get; set; }

        /// <summary>
        /// 获取或设置 图片高度
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// 获取或设置 背景颜色
        /// </summary>
        public Color BgColor { get; set; }

        /// <summary>
        /// 获取或设置 是否有边框
        /// </summary>
        public bool HasBorder { get; set; }

        /// <summary>
        /// 获取或设置 是否随机位置
        /// </summary>
        public bool RandomPosition { get; set; }

        /// <summary>
        /// 获取或设置 是否随机字体颜色
        /// </summary>
        public bool RandomColor { get; set; }

        /// <summary>
        /// 获取或设置 是否随机倾斜字体
        /// </summary>
        public bool RandomItalic { get; set; }

        /// <summary>
        /// 获取或设置 随机干扰点百分比（百分数形式）
        /// </summary>
        public double RandomPointPercent { get; set; }

        /// <summary>
        /// 获取或设置 随机干扰线数量
        /// </summary>
        public int RandomLineCount { get; set; }

        #endregion

        /// <summary>
        /// 获取指定长度的验证码字符串
        /// </summary>
        public string GetCode(int length, ValidateCodeType codeType = ValidateCodeType.NumberAndLetter)
        {
            Check.GreaterThan(length, nameof(length), 0);
            switch (codeType)
            {
                case ValidateCodeType.Number:
                    return GetRandomNums(length);
                case ValidateCodeType.Hanzi:
                    return GetRandomHanzis(length);
                default:
                    return GetRandomNumsAndLetters(length);
            }
        }

        /// <summary>
        /// 获取指定长度的验证码图片
        /// </summary>
        public Image CreateImage(int length, out string code, ValidateCodeType codeType = ValidateCodeType.NumberAndLetter)
        {
            Check.GreaterThan(length, nameof(length), 0);

            switch (codeType)
            {
                case ValidateCodeType.Number:
                    code = GetRandomNums(length);
                    break;
                case ValidateCodeType.Hanzi:
                    code = GetRandomHanzis(length);
                    break;
                default:
                    code = GetRandomNumsAndLetters(length);
                    break;
            }
            if (code.Length > length)
            {
                code = code.Substring(0, length);
            }
            return CreateImage(code, codeType);
        }

        /// <summary>
        /// 获取指定字符串的验证码图片
        /// </summary>
        public Image CreateImage(string code, ValidateCodeType codeType)
        {
            Check.NotNullOrEmpty(code, nameof(code));

            int width = FontWidth * code.Length + FontWidth,
                height = FontSize + FontSize / 2,
                flag = 255 / 2;
            bool isBgLight = (BgColor.R + BgColor.G + BgColor.B) / 3 > flag;
            Image image = new Bitmap(null, width, height);


















            throw new NotImplementedException();
        }

        private string GetRandomNums(int length)
        {
            int[] nums = new int[length];
            for (int i = 0; i < length; i++)
            {
                nums[i] = Random.Next(0, 9);
            }
            return nums.ExpandAndToString("");
        }

        private string GetRandomNumsAndLetters(int length)
        {
            const string allChar = "2,3,4,5,6,7,8,9," +
                "A,B,C,D,E,F,G,H,J,K,M,N,P,Q,R,S,T,U,V,W,X,Y,Z," +
                "a,b,c,d,e,f,g,h,k,m,n,p,q,r,s,t,u,v,w,x,y,z";
            string[] allChars = allChar.Split(',');
            List<string> result = new List<string>();
            while (result.Count < length)
            {
                int index = Random.Next(allChars.Length);
                string c = allChars[index];
                result.Add(c);
            }
            return result.ExpandAndToString("");
        }

        private string GetRandomHanzis(int length)
        {
            //汉字编码的组成元素，十六进制数
            string[] baseStrs = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f".Split(',');
            Encoding encoding = Encoding.GetEncoding("GB2312");
            string result = null;

            //每循环一次产生一个含两个元素的十六进制字节数组，并放入bytes数组中
            //汉字由四个区位码组成，1、2位作为字节数组的第一个元素，3、4位作为第二个元素
            for (int i = 0; i < length; i++)
            {
                Random rnd = Random;
                int index1 = rnd.Next(11, 14);
                string str1 = baseStrs[index1];

                int index2 = index1 == 13 ? rnd.Next(0, 7) : rnd.Next(0, 16);
                string str2 = baseStrs[index2];

                int index3 = rnd.Next(10, 16);
                string str3 = baseStrs[index3];

                int index4 = index3 == 10 ? rnd.Next(1, 16) : (index3 == 15 ? rnd.Next(0, 15) : rnd.Next(0, 16));
                string str4 = baseStrs[index4];

                //定义两个字节变量存储产生的随机汉字区位码
                byte b1 = Convert.ToByte(str1 + str2, 16);
                byte b2 = Convert.ToByte(str3 + str4, 16);
                byte[] bs = { b1, b2 };

                result += encoding.GetString(bs);
            }
            return result;
        }
    }
}