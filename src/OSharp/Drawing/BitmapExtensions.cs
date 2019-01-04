// -----------------------------------------------------------------------
//  <copyright file="BitmapExtensions.cs" company="柳柳软件">
//      Copyright (c) 2016-2018 66SOFT. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-10-02 2:42</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

using OSharp.Collections;
using OSharp.Extensions;


namespace OSharp.Drawing
{
    /// <summary>
    /// 图像扩展方法
    /// </summary>
    public static class BitmapExtensions
    {
        #region Byte[,]图像处理扩展

        /// <summary>
        /// 将图像转换为 Color[,]颜色值二维数组
        /// </summary>
        public static Color[,] ToPixelArray2D(this Bitmap bmp)
        {
            int width = bmp.Width, height = bmp.Height;
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* ptr = (byte*)data.Scan0;
                Color[,] pixels = new Color[width, height];
                int offset = data.Stride - width * 3;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        pixels[x, y] = Color.FromArgb(ptr[2], ptr[1], ptr[0]);
                        ptr += 3;
                    }
                    ptr += offset;
                }
                return pixels;
            }
        }

        /// <summary>
        /// 将图像转换为 Byte[,]灰度值二维数组，后续所有操作都将以二维数组作为中间变量
        /// </summary>
        public static byte[,] ToGrayArray2D(this Bitmap bmp)
        {
            int width = bmp.Width, height = bmp.Height;
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* ptr = (byte*)data.Scan0;
                byte[,] grayBytes = new byte[width, height];
                int offset = data.Stride - width * 3;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        grayBytes[x, y] = GetGrayValue(ptr[2], ptr[1], ptr[0]);
                        ptr += 3;
                    }
                    ptr += offset;
                }
                bmp.UnlockBits(data);
                return grayBytes;
            }
        }

        /// <summary>
        /// 将颜色数组二维数组转换为灰度值二维数组
        /// </summary>
        public static byte[,] ToGrayArray2D(this Color[,] pixels)
        {
            int width = pixels.GetLength(0), height = pixels.GetLength(1);
            byte[,] grayBytes = new byte[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    grayBytes[x, y] = GetGrayValue(pixels[x, y]);
                }
            }
            return grayBytes;
        }

        /// <summary>
        /// 将二维颜色数组转换为图像
        /// </summary>
        public static Bitmap ToBitmap(this Color[,] pixels)
        {
            int width = pixels.GetLength(0), height = pixels.GetLength(1);
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* ptr = (byte*)data.Scan0;
                int offset = data.Stride - width * 3;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        Color pixel = pixels[x, y];
                        ptr[2] = pixel.R;
                        ptr[1] = pixel.G;
                        ptr[0] = pixel.B;
                        ptr += 3;
                    }
                    ptr += offset;
                }
                bmp.UnlockBits(data);
                return bmp;
            }
        }

        /// <summary>
        /// 将二维灰度数组转换为图像
        /// </summary>
        public static Bitmap ToBitmap(this byte[,] grayBytes)
        {
            int width = grayBytes.GetLength(0), height = grayBytes.GetLength(1);
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* ptr = (byte*)data.Scan0;
                int offset = data.Stride - width * 3;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        ptr[2] = ptr[1] = ptr[0] = grayBytes[x, y];
                        ptr += 3;
                    }
                    ptr += offset;
                }
                bmp.UnlockBits(data);
                return bmp;
            }
        }

        /// <summary>
        /// 将二维灰度数组二值化
        /// </summary>
        public static byte[,] Binaryzation(this byte[,] grayBytes, byte gray)
        {
            int width = grayBytes.GetLength(0), height = grayBytes.GetLength(1);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    grayBytes[x, y] = (byte)(grayBytes[x, y] > gray ? 255 : 0);
                }
            }
            return grayBytes;
        }

        /// <summary>
        /// 将二维灰度数组前景色加黑
        /// </summary>
        public static byte[,] DeepFore(this byte[,] grayBytes, byte gray = 200)
        {
            int width = grayBytes.GetLength(0), height = grayBytes.GetLength(1);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (grayBytes[x, y] < gray)
                    {
                        grayBytes[x, y] = 0;
                    }
                }
            }
            return grayBytes;
        }

        /// <summary>
        /// 去除噪音，周边有效点数的方式（适合杂点/细线）
        /// </summary>
        public static byte[,] ClearNoiseRound(this byte[,] binBytes, byte gray, int maxNearPoints)
        {
            int width = binBytes.GetLength(0), height = binBytes.GetLength(1);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    byte value = binBytes[x, y];
                    //背景，边框
                    if (value > gray || (x == 0 || y == 0 || x == width - 1 || y == height - 1))
                    {
                        binBytes[x, y] = 255;
                        continue;
                    }
                    int count = 0;
                    if (binBytes[x - 1, y - 1] < gray) count++;
                    if (binBytes[x, y - 1] < gray) count++;
                    if (binBytes[x + 1, y - 1] < gray) count++;
                    if (binBytes[x, y - 1] < gray) count++;
                    if (binBytes[x, y + 1] < gray) count++;
                    if (binBytes[x - 1, y + 1] < gray) count++;
                    if (binBytes[x, y + 1] < gray) count++;
                    if (binBytes[x + 1, y + 1] < gray) count++;
                    //如果周边有效点数小于指定阈值，则清除该点
                    if (count < maxNearPoints)
                    {
                        binBytes[x, y] = 255;
                    }
                }
            }
            return binBytes;
        }

        /// <summary>
        /// 去除噪音，连通域降噪方式，去除连通点数小于阈值的连通区域
        /// </summary>
        public static byte[,] ClearNoiseArea(this byte[,] binBytes, byte gray, int minAreaPoints)
        {
            int width = binBytes.GetLength(0), height = binBytes.GetLength(1);
            byte[,] newBinBytes = binBytes.Copy();
            //遍历所有点，是黑点0，把与黑点连通的所有点灰度都改为1，下一个连通区域改为2，直到所有连通区域都标记完毕
            Dictionary<byte, Point[]> areaPointDict = new Dictionary<byte, Point[]>();
            byte setGray = 1;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (IsBlack(newBinBytes[x, y]))
                    {
                        Point[] setPoints;
                        newBinBytes.FloodFill(new Point(x, y), setGray, out setPoints);
                        areaPointDict.Add(setGray, setPoints);

                        setGray++;
                        if (setGray >= 255)
                        {
                            setGray = 254;
                        }
                    }
                }
            }
            //筛选出区域点数小于阈值的区域，将原图相应点设置为白色
            List<Point[]> pointsList = areaPointDict.Where(m => m.Value.Length < minAreaPoints).Select(m => m.Value).ToList();
            foreach (Point[] points in pointsList)
            {
                foreach (Point point in points)
                {
                    binBytes[point.X, point.Y] = 255;
                }
            }

            return binBytes;
        }

        /// <summary>
        /// 泛水填充算法，将相连通的区域使用指定灰度值填充
        /// </summary>
        public static byte[,] FloodFill(this byte[,] binBytes, Point point, byte replacementGray)
        {
            int width = binBytes.GetLength(0), height = binBytes.GetLength(1);
            Stack<Point> stack = new Stack<Point>();
            byte gray = binBytes[point.X, point.Y];
            stack.Push(point);

            while (stack.Count > 0)
            {
                var p = stack.Pop();
                if (p.X <= 0 || p.X >= width || p.Y <= 0 || p.Y >= height)
                {
                    continue;
                }
                if (binBytes[p.X, p.Y] == gray)
                {
                    binBytes[p.X, p.Y] = replacementGray;

                    stack.Push(new Point(p.X - 1, p.Y));
                    stack.Push(new Point(p.X + 1, p.Y));
                    stack.Push(new Point(p.X, p.Y - 1));
                    stack.Push(new Point(p.X, p.Y + 1));
                }
            }

            return binBytes;
        }

        /// <summary>
        /// 泛水填充算法，将相连通的区域使用指定灰度值填充
        /// </summary>
        public static byte[,] FloodFill(this byte[,] binBytes, Point point, byte replacementGray, out Point[] points)
        {
            int width = binBytes.GetLength(0), height = binBytes.GetLength(1);
            List<Point> pointList = new List<Point>();
            Stack<Point> stack = new Stack<Point>();
            byte gray = binBytes[point.X, point.Y];
            stack.Push(point);

            while (stack.Count > 0)
            {
                var p = stack.Pop();
                if (p.X <= 0 || p.X >= width || p.Y <= 0 || p.Y >= height)
                {
                    continue;
                }
                if (binBytes[p.X, p.Y] == gray)
                {
                    binBytes[p.X, p.Y] = replacementGray;
                    pointList.Add(p);

                    stack.Push(new Point(p.X - 1, p.Y));
                    stack.Push(new Point(p.X + 1, p.Y));
                    stack.Push(new Point(p.X, p.Y - 1));
                    stack.Push(new Point(p.X, p.Y + 1));
                }
            }

            points = pointList.ToArray();
            return binBytes;
        }

        /// <summary>
        /// 去除图片边框
        /// </summary>
        public static byte[,] ClearBorder(this byte[,] grayBytes, int border)
        {
            int width = grayBytes.GetLength(0), height = grayBytes.GetLength(1);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (x < border || y < border || x > width - 1 - border || y > height - 1 - border)
                    {
                        grayBytes[x, y] = 255;
                    }
                }
            }
            return grayBytes;
        }

        /// <summary>
        /// 给图片添加边框，默认白色
        /// </summary>
        public static byte[,] AddBorder(this byte[,] grayBytes, int border, byte gray = 255)
        {
            int width = grayBytes.GetLength(0) + border * 2, height = grayBytes.GetLength(1) + border * 2;
            byte[,] newBytes = new byte[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (x < border || y < border || x > width - 1 - border || y > height - 1 - border)
                    {
                        newBytes[x, y] = gray;
                    }
                }
            }
            newBytes = grayBytes.DrawTo(newBytes, border, border);
            return newBytes;
        }

        /// <summary>
        /// 去除指定范围的灰度
        /// </summary>
        public static byte[,] ClearGray(this byte[,] grayBytes, byte minGray, byte maxGray)
        {
            int width = grayBytes.GetLength(0), height = grayBytes.GetLength(1);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    byte value = grayBytes[x, y];
                    if (minGray <= value && value <= maxGray)
                    {
                        grayBytes[x, y] = 255;
                    }
                }
            }
            return grayBytes;
        }

        /// <summary>
        /// 去除空白边界获取有效的图形
        /// </summary>
        public static byte[,] ToValid(this byte[,] binBytes, byte gray = 200)
        {
            int width = binBytes.GetLength(0), height = binBytes.GetLength(1);
            //有效矩形的左上/右下角坐标，左上坐标从右下开始拉，右下坐标从左上开始拉，所以初始值为
            int x1 = width, y1 = height, x2 = 0, y2 = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    byte value = binBytes[x, y];
                    if (value >= gray)
                    {
                        continue;
                    }
                    if (x1 > x) x1 = x;
                    if (y1 > y) y1 = y;
                    if (x2 < x) x2 = x;
                    if (y2 < y) y2 = y;
                }
            }
            //创建新矩阵，复制原数据到新矩阵
            int newWidth = x2 - x1 + 1, newHeight = y2 - y1 + 1;
            byte[,] newBytes = binBytes.Clone(x1, y1, newWidth, newHeight);
            return newBytes;
        }

        /// <summary>
        /// 从原矩阵中复制指定矩阵
        /// </summary>
        public static byte[,] Clone(this byte[,] sourceBytes, int x1, int y1, int width, int height)
        {
            int swidth = sourceBytes.GetLength(0), sheight = sourceBytes.GetLength(1);
            if (swidth - x1 < width)
            {
                throw new ArgumentException("要截取的宽度超出界限");
            }
            if (sheight - y1 < height)
            {
                throw new ArgumentException("要截取的高度超出界限");
            }
            byte[,] newBytes = new byte[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    newBytes[x, y] = sourceBytes[x1 + x, y1 + y];
                }
            }
            return newBytes;
        }

        /// <summary>
        /// 将小图画到大图中
        /// </summary>
        public static byte[,] DrawTo(this byte[,] smallBytes, byte[,] bigBytes, int x1, int y1)
        {
            int smallWidth = smallBytes.GetLength(0),
                smallHeight = smallBytes.GetLength(1),
                bigWidth = bigBytes.GetLength(0),
                bigHeight = bigBytes.GetLength(1);
            if (x1 + smallWidth > bigWidth)
            {
                throw new ArgumentException("大图矩阵宽度无法装下小矩阵宽度");
            }
            if (y1 + smallHeight > bigHeight)
            {
                throw new ArgumentException("大图矩阵高度无法装下小矩阵高度");
            }
            for (int y = 0; y < smallHeight; y++)
            {
                for (int x = 0; x < smallWidth; x++)
                {
                    bigBytes[x1 + x, y1 + y] = smallBytes[x, y];
                }
            }

            return bigBytes;
        }

        /// <summary>
        /// 统计二维二值化数组的的竖直投影
        /// </summary>
        public static int[] ShadowY(this byte[,] binBytes)
        {
            int width = binBytes.GetLength(0), height = binBytes.GetLength(1);
            int[] nums = new int[width];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (IsBlack(binBytes[x, y]))
                    {
                        nums[x]++;
                    }
                }
            }
            return nums;
        }

        /// <summary>
        /// 统计二维二值化数组的横向投影
        /// </summary>
        public static int[] ShadowX(this byte[,] binBytes)
        {
            int width = binBytes.GetLength(0), height = binBytes.GetLength(1);
            int[] nums = new int[height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (IsBlack(binBytes[x, y]))
                    {
                        nums[y]++;
                    }
                }
            }
            return nums;
        }

        /// <summary>
        /// 根据二值化数组的竖直投影数据分割图片
        /// </summary>
        /// <param name="binBytes">二维二值化数组</param>
        /// <param name="minFontWidth">最小字符宽度，0则自动</param>
        /// <param name="minLines">最小有效投影行数</param>
        /// <returns></returns>
        public static List<byte[,]> SplitShadowY(this byte[,] binBytes, byte minFontWidth = 0, byte minLines = 0)
        {
            int height = binBytes.GetLength(1);
            int[] shadow = binBytes.ShadowY();
            List<Tuple<int, int>> validXs = new List<Tuple<int, int>>();
            int x1 = 0;
            bool inFont = false;
            for (int x = 0; x < shadow.Length; x++)
            {
                int value = shadow[x];
                if (!inFont)
                {
                    if (value > minLines)
                    {
                        inFont = true;
                        x1 = x;
                    }
                }
                else
                {
                    if (value <= minLines)
                    {
                        inFont = false;
                        if (minFontWidth == 0 || x - x1 > minFontWidth)
                        {
                            validXs.Add(new Tuple<int, int>(x1, x));
                        }
                    }
                }
            }

            List<byte[,]> splits = validXs.Select(valid => binBytes.Clone(valid.Item1, 0, valid.Item2 - valid.Item1 + 1, height).ToValid()).ToList();
            return splits;
        }

        /// <summary>
        /// 将二维二值化数组转换为特征码字符串
        /// </summary>
        public static string ToCodeString(this byte[,] binBytes, byte gray, bool breakLine = false)
        {
            int width = binBytes.GetLength(0), height = binBytes.GetLength(1);
            string code = string.Empty;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    code += binBytes[x, y] < gray ? 1 : 0;
                }
                if (breakLine)
                {
                    code += "\r\n";
                }
            }
            return code;
        }

        #endregion

        #region Image

        /// <summary>
        /// 将Bitmap转换为Byte[]
        /// </summary>
        /// <param name="bmp">待处理的图像</param>
        /// <returns></returns>
        public static byte[] ToBytes(this Bitmap bmp)
        {
            using (Bitmap newBmp = new Bitmap(bmp))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    ImageFormat format = newBmp.RawFormat;
                    if (ImageFormat.MemoryBmp.Equals(format))
                    {
                        format = ImageFormat.Bmp;
                    }
                    newBmp.Save(ms, format);
                    return ms.GetBuffer();
                }
            }
        }

        /// <summary>
        /// 使图像绕中心点旋转一定角度
        /// </summary>
        /// <param name="bmp">待处理的图像</param>
        /// <param name="angle">旋转的角度，正值为逆时针方向</param>
        /// <returns> 旋转后的图像 </returns>
        public static Bitmap Rotate(this Bitmap bmp, int angle)
        {
            angle = angle % 360;

            //弧度转换
            double radian = angle * Math.PI / 180.0;
            double cos = Math.Cos(radian);
            double sin = Math.Sin(radian);

            //原图的宽和高
            int w1 = bmp.Width;
            int h1 = bmp.Height;
            //旋转后的宽和高
            int w2 = (int)Math.Max(Math.Abs(w1 * cos - h1 * sin), Math.Abs(w1 * cos + h1 * sin));
            int h2 = (int)Math.Max(Math.Abs(w1 * sin - h1 * cos), Math.Abs(w1 * sin + h1 * cos));

            Bitmap newBmp = new Bitmap(w2, h2);
            using (Graphics graphics = Graphics.FromImage(newBmp))
            {
                //目标位图
                graphics.InterpolationMode = InterpolationMode.Bilinear;
                graphics.SmoothingMode = SmoothingMode.HighQuality;

                //计算偏移量
                Point offset = new Point((w2 - w1) / 2, (h2 - h1) / 2);

                //构造图像显示区域：使原始图像与目标图像中心点一致
                Rectangle rect = new Rectangle(offset.X, offset.Y, w1, h1);
                Point center = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);

                graphics.TranslateTransform(center.X, center.Y);
                graphics.RotateTransform(360 - angle);

                //恢复图像在水平和垂直方向的平移
                graphics.TranslateTransform(-center.X, -center.Y);
                graphics.DrawImage(bmp, rect);

                //重置绘图的所有变换
                graphics.ResetTransform();
                graphics.Save();
                graphics.Dispose();
                return newBmp;
            }
        }

        /// <summary>
        /// 对一个坐标点按照一个中心进行旋转
        /// </summary>
        public static Point Rotate(this Point center, Point point, int angle)
        {
            angle = angle % 360;

            //弧度转换
            double radian = angle * Math.PI / 180.0;
            double cos = Math.Cos(radian), sin = Math.Sin(radian);

            double x = (point.X - center.X) * cos + (point.Y - center.Y) * sin + center.X;
            double y = (point.X - center.X) * sin + (point.Y - center.Y) * cos + center.Y;

            return new Point((int)Math.Round(x, 0), (int)Math.Round(y, 0));
        }

        /// <summary>
        /// 按指定宽度与高度缩放图像
        /// </summary>
        /// <param name="bmp">待处理的图像</param>
        /// <param name="width">缩放后的宽度</param>
        /// <param name="height">缩放后的高度</param>
        /// <param name="model">图像质量模式</param>
        /// <returns> 缩放后的图像 </returns>
        public static Bitmap Zoom(this Bitmap bmp, int width, int height, InterpolationMode model = InterpolationMode.Default)
        {
            Bitmap newBmp = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(newBmp))
            {
                graphics.InterpolationMode = model;
                graphics.DrawImage(bmp, new Rectangle(0, 0, width, height), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                return newBmp;
            }
        }

        /// <summary>
        /// 按指定百分比缩放图像
        /// </summary>
        /// <param name="bmp">待处理的图像</param>
        /// <param name="percent">缩放百分比（小数）</param>
        /// <param name="model">图像质量模式</param>
        /// <returns> 缩放后的图像 </returns>
        public static Bitmap Zoom(this Bitmap bmp, double percent, InterpolationMode model = InterpolationMode.Default)
        {
            int width = (int)(bmp.Width * percent);
            int height = (int)(bmp.Height * percent);
            return Zoom(bmp, width, height, model);
        }

        /// <summary>
        /// 图像灰度化，逐点方式
        /// </summary>
        /// <param name="bmp">待处理的图像</param>
        /// <returns> 灰度化后的图像 </returns>
        public static Bitmap GrayByPixels(this Bitmap bmp)
        {
            Bitmap newBmp = new Bitmap(bmp.Width, bmp.Height);
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    Color pixel = bmp.GetPixel(x, y);
                    byte value = GetGrayValue(pixel);
                    newBmp.SetPixel(x, y, Color.FromArgb(value, value, value));
                }
            }
            return newBmp;
        }

        /// <summary>
        /// 图像灰度化，逐行扫描方式
        /// </summary>
        public static Bitmap GrayByLine(this Bitmap bmp)
        {
            int width = bmp.Width, height = bmp.Height;
            Bitmap newBmp = bmp.Clone(new Rectangle(0, 0, bmp.Width, bmp.Height), bmp.PixelFormat);
            BitmapData data = newBmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* ptr = (byte*)data.Scan0;
                int offset = data.Stride - width * 3;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        ptr[0] = ptr[1] = ptr[2] = GetGrayValue(ptr[2], ptr[1], ptr[0]);
                        ptr += 3;
                    }
                    ptr += offset;
                }
                newBmp.UnlockBits(data);
            }
            return newBmp;
        }

        /// <summary>
        /// 图像前景色加黑
        /// </summary>
        /// <param name="bmp">待处理的图像</param>
        /// <param name="gray">指定灰度阈值，灰度小于该值，则设置为黑色</param>
        /// <returns> 深化后的图像 </returns>
        public static Bitmap DeepFore(this Bitmap bmp, byte gray = 200)
        {
            Bitmap newBmp = new Bitmap(bmp.Width, bmp.Height);
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    Color pixel = bmp.GetPixel(i, j);
                    if (pixel.R < gray)
                    {
                        newBmp.SetPixel(i, j, Color.Black);
                    }
                }
            }
            return newBmp;
        }

        /// <summary>
        /// 去掉杂点，周边有效点数的方式(适合杂点/杂线粗为1)
        /// </summary>
        /// <param name="bmp">等处理图像</param>
        /// <param name="gray">临界灰度值，大于此值的点将视为无效点</param>
        /// <param name="maxNearPoints">周边最大有效点数，有效点数小于此值，当前点视为杂点，取值范围[0,4]</param>
        /// <returns></returns>
        public static Bitmap ClearNoise(this Bitmap bmp, byte gray, int maxNearPoints)
        {
            maxNearPoints.CheckBetween("maxNearPoints", 1, 4, true, true);
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color piexl = bmp.GetPixel(x, y);
                    //背景，边框
                    if (piexl.R >= gray || (x == 0 || x == bmp.Width - 1 || y == 0 || y == bmp.Height - 1))
                    {
                        bmp.SetPixel(x, y, Color.White);
                        continue;
                    }
                    int count = 0;
                    if (bmp.GetPixel(x - 1, y - 1).R < gray) count++;
                    if (bmp.GetPixel(x, y - 1).R < gray) count++;
                    if (bmp.GetPixel(x + 1, y - 1).R < gray) count++;
                    if (bmp.GetPixel(x - 1, y).R < gray) count++;
                    if (bmp.GetPixel(x + 1, y).R < gray) count++;
                    if (bmp.GetPixel(x - 1, y + 1).R < gray) count++;
                    if (bmp.GetPixel(x, y + 1).R < gray) count++;
                    if (bmp.GetPixel(x + 1, y + 1).R < gray) count++;
                    //如果周边有效点数小于指定阈值，则清除该点
                    if (count < maxNearPoints)
                    {
                        bmp.SetPixel(x, y, Color.White);
                    }
                }
            }
            return bmp.Clone(new Rectangle(0, 0, bmp.Width, bmp.Height), bmp.PixelFormat);
        }

        /// <summary>
        /// 调整图像亮度
        /// </summary>
        /// <param name="bmp">待处理的图像</param>
        /// <param name="value">调整的亮度值，取值为[-255, 255]</param>
        /// <returns> 调整亮度后的图像 </returns>
        public static Bitmap Brightness(this Bitmap bmp, int value)
        {
            value = value < -255 ? -255 : value;
            value = value > 255 ? 255 : value;
            int width = bmp.Width, height = bmp.Height;
            Bitmap newBmp = bmp.Clone(new Rectangle(0, 0, width, height), bmp.PixelFormat);
            BitmapData bmpData = newBmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* p = (byte*)bmpData.Scan0;
                int offset = bmpData.Stride - width * 3;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        // 处理指定位置像素的亮度
                        for (int i = 0; i < 3; i++)
                        {
                            int pix = p[i] + value;
                            if (value < 0)
                            {
                                p[i] = (byte)Math.Max(0, pix);
                            }
                            if (value > 0)
                            {
                                p[i] = (byte)Math.Min(255, pix);
                            }
                        } // i
                        p += 3;
                    } // x
                    p += offset;
                } // y
            }

            newBmp.UnlockBits(bmpData);
            return newBmp;
        }

        /// <summary>
        /// 调整图像对比度
        /// </summary>
        /// <param name="bmp">待处理的图像</param>
        /// <param name="value">调整的对比度，取值为[-100, 100]</param>
        /// <returns> 调整对比度后的图像 </returns>
        public static Bitmap Contrast(this Bitmap bmp, int value)
        {
            value = value < -100 ? -100 : value;
            value = value > 100 ? 100 : value;
            double contrast = (100.0 + value) / 100.0;
            contrast *= contrast;
            int width = bmp.Width, height = bmp.Height;
            Bitmap newBmp = bmp.Clone(new Rectangle(0, 0, width, height), bmp.PixelFormat);
            BitmapData bmpData = newBmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* p = (byte*)bmpData.Scan0;
                int offset = bmpData.Stride - width * 3;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        // 处理指定位置像素的对比度
                        for (int i = 0; i < 3; i++)
                        {
                            double pixel = ((p[i] / 255.0 - 0.5) * contrast + 0.5) * 255;
                            pixel = pixel < 0 ? 0 : pixel;
                            pixel = pixel > 255 ? 255 : pixel;
                            p[i] = (byte)pixel;
                        } // i
                        p += 3;
                    } // x
                    p += offset;
                } // y
            }
            newBmp.UnlockBits(bmpData);
            return newBmp;
        }

        /// <summary>
        /// Gamma校正
        /// </summary>
        /// <param name="bmp">待处理的图像</param>
        /// <param name="value">Gamma值</param>
        /// <returns> Gamma校正后的图像 </returns>
        public static Bitmap Gamma(this Bitmap bmp, float value)
        {
            if (Equals(value, 1.0000f))
            {
                return bmp;
            }
            Bitmap newBmp = new Bitmap(bmp.Width, bmp.Height);
            using (Graphics graphics = Graphics.FromImage(newBmp))
            {
                ImageAttributes attribtues = new ImageAttributes();
                attribtues.SetGamma(value, ColorAdjustType.Bitmap);
                graphics.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, attribtues);
                return newBmp;
            }
        }

        /// <summary>
        /// 在图片上打印文字
        /// </summary>
        /// <param name="bmp">待处理的图像</param>
        /// <param name="text">要打印的文字</param>
        /// <param name="font">字体信息</param>
        /// <param name="color">文字颜色</param>
        /// <param name="x">文字位置横坐标</param>
        /// <param name="y">文字位置纵坐标</param>
        /// <returns> 打印文字后的图像 </returns>
        public static Bitmap SetText(this Bitmap bmp, string text, Font font, Color color, int x, int y)
        {
            Bitmap newBmp = bmp.Clone(new Rectangle(0, 0, bmp.Width, bmp.Height), bmp.PixelFormat);
            using (Graphics graphics = Graphics.FromImage(newBmp))
            {
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                SolidBrush brush = new SolidBrush(color);
                graphics.DrawString(text, font, brush, new PointF(x, y));
                return newBmp;
            }
        }

        /// <summary>
        /// 去除图片边框
        /// </summary>
        /// <param name="bmp">待处理的图像</param>
        /// <param name="border">边框宽度</param>
        /// <returns></returns>
        public static Bitmap ClearBorder(this Bitmap bmp, int border)
        {
            int width = bmp.Width, height = bmp.Height;
            Bitmap newBmp = bmp.Clone(new Rectangle(0, 0, width, height), bmp.PixelFormat);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (x < border || y < border || x > width - 1 - border || y > height - 1 - border)
                    {
                        newBmp.SetPixel(x, y, Color.White);
                    }
                }
            }

            return newBmp;
        }

        /// <summary>
        /// 底片效果
        /// </summary>
        /// <param name="bmp">待处理的图像</param>
        /// <returns> 底片效果的图像 </returns>
        public static Bitmap Plate(this Bitmap bmp)
        {
            int width = bmp.Width, height = bmp.Height;
            Bitmap newBmp = new Bitmap(width, height);
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    Color pixel = bmp.GetPixel(i, j);
                    int r = 255 - pixel.R;
                    int g = 255 - pixel.G;
                    int b = 255 - pixel.B;
                    newBmp.SetPixel(i, j, Color.FromArgb(r, g, b));
                }
            }
            return newBmp;
        }

        /// <summary>
        /// 浮雕效果
        /// </summary>
        /// <param name="bmp">待处理的图像</param>
        /// <returns> 浮雕效果的图像 </returns>
        public static Bitmap Emboss(this Bitmap bmp)
        {
            int width = bmp.Width, height = bmp.Height;
            Bitmap newBmp = new Bitmap(width, height);
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    Color pixel1 = bmp.GetPixel(i, j);
                    Color pixel2 = bmp.GetPixel(i + 1, j + 1);
                    int r = Math.Abs(pixel1.R - pixel2.R + 128);
                    int g = Math.Abs(pixel1.G - pixel2.G + 128);
                    int b = Math.Abs(pixel1.B - pixel2.B + 128);
                    r = r > 255 ? 255 : r;
                    r = r < 0 ? 0 : r;
                    g = g > 255 ? 255 : g;
                    g = g < 0 ? 0 : g;
                    b = b > 255 ? 255 : b;
                    b = b < 0 ? 0 : b;
                    newBmp.SetPixel(i, j, Color.FromArgb(r, g, b));
                }
            }
            return newBmp;
        }

        /// <summary>
        /// 柔化效果
        /// </summary>
        /// <param name="bmp">待处理的图像</param>
        /// <returns> 柔化效果的图像 </returns>
        public static Bitmap Soften(this Bitmap bmp)
        {
            int width = bmp.Width, height = bmp.Height;
            Bitmap newBmp = new Bitmap(width, height);
            //高斯模板
            int[] gauss = { 1, 2, 1, 2, 4, 2, 1, 2, 1 };
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    int index = 0;
                    int r = 0, g = 0, b = 0;
                    for (int col = -1; col <= 1; col++)
                    {
                        for (int row = -1; row <= 1; row++)
                        {
                            Color pixel = bmp.GetPixel(i + row, j + col);
                            r += pixel.R * gauss[index];
                            g += pixel.G * gauss[index];
                            b += pixel.B * gauss[index];
                            index++;
                        }
                    }
                    r /= 16;
                    g /= 16;
                    b /= 16;
                    //处理颜色值溢出
                    r = r > 255 ? 255 : r;
                    r = r < 0 ? 0 : r;
                    g = g > 255 ? 255 : g;
                    g = g < 0 ? 0 : g;
                    b = b > 255 ? 255 : b;
                    b = b < 0 ? 0 : b;
                    newBmp.SetPixel(i - 1, j - 1, Color.FromArgb(r, g, b));
                }
            }
            return newBmp;
        }

        /// <summary>
        /// 锐化效果
        /// </summary>
        /// <param name="bmp">待处理的图像</param>
        /// <returns> 锐化效果的图像 </returns>
        public static Bitmap Sharpen(this Bitmap bmp)
        {
            int width = bmp.Width, height = bmp.Height;
            Bitmap newBmp = new Bitmap(width, height);
            //拉普拉斯模板
            int[] laplacian = { -1, -1, -1, -1, 9, -1, -1, -1, -1 };
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    int index = 0;
                    int r = 0, g = 0, b = 0;
                    for (int col = -1; col <= 1; col++)
                    {
                        for (int row = -1; row <= 1; row++)
                        {
                            Color pixel = bmp.GetPixel(i + row, j + col);
                            r += pixel.R * laplacian[index];
                            g += pixel.G * laplacian[index];
                            b += pixel.B * laplacian[index];
                            index++;
                        }
                    }
                    r /= 16;
                    g /= 16;
                    b /= 16;
                    //处理颜色值溢出
                    r = r > 255 ? 255 : r;
                    r = r < 0 ? 0 : r;
                    g = g > 255 ? 255 : g;
                    g = g < 0 ? 0 : g;
                    b = b > 255 ? 255 : b;
                    b = b < 0 ? 0 : b;
                    newBmp.SetPixel(i - 1, j - 1, Color.FromArgb(r, g, b));
                }
            }
            return newBmp;
        }

        /// <summary>
        /// 雾化效果
        /// </summary>
        /// <param name="bmp">待处理的图像</param>
        /// <returns> 雾化效果的图像 </returns>
        public static Bitmap Atomizing(this Bitmap bmp)
        {
            int width = bmp.Width, height = bmp.Height;
            Bitmap newBmp = new Bitmap(width, height);
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    Random rnd = new Random();
                    int k = rnd.Next(123456);
                    //像素块大小
                    int dx = i + k % 19;
                    int dy = j + k % 19;
                    if (dx >= width)
                    {
                        dx = width - 1;
                    }
                    if (dy >= height)
                    {
                        dy = height - 1;
                    }
                    Color pixel = bmp.GetPixel(dx, dy);
                    newBmp.SetPixel(i, j, pixel);
                }
            }
            return newBmp;
        }

        /// <summary>
        /// 二值化效果
        /// </summary>
        public static Bitmap Binaryzation(this Bitmap bmp)
        {
            int width = bmp.Width, height = bmp.Height;
            Bitmap newBmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            BitmapData data = newBmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format1bppIndexed);
            for (int j = 0; j < height; j++)
            {
                byte[] scan = new byte[(width + 7) / 8];
                for (int i = 0; i < width; i++)
                {
                    Color pixel = bmp.GetPixel(i, j);
                    if (pixel.GetBrightness() >= 0.5)
                    {
                        scan[i / 8] |= (byte)(0x80 >> (i % 8));
                    }
                }
                Marshal.Copy(scan, 0, (IntPtr)((int)data.Scan0 + data.Stride * j), scan.Length);
            }
            newBmp.UnlockBits(data);
            return newBmp;
        }

        /// <summary>
        /// 固定阈值的二值化
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="threshold"></param>
        /// <returns> </returns>
        public static Bitmap Binaryzation(this Bitmap bmp, byte threshold)
        {
            int width = bmp.Width, height = bmp.Height;
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            unsafe
            {
                //将原始图片变成灰度二维数组
                byte* ptr = (byte*)data.Scan0;
                byte[,] source = new byte[width, height];
                int offset = data.Stride - width * 3;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        source[x, y] = GetGrayValue(ptr[2], ptr[1], ptr[0]);
                        ptr += 3;
                    }
                    ptr += offset;
                }
                bmp.UnlockBits(data);
                //将灰度二位数组转换为二值图像
                Bitmap newBmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                BitmapData newData = newBmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
                ptr = (byte*)newData.Scan0;
                offset = newData.Stride - width * 3;
                for (int j = 0; j < height; j++)
                {
                    for (int i = 0; i < width; i++)
                    {
                        ptr[0] = ptr[1] = ptr[2] = GetAverageColor(source, i, j, width, height) > threshold ? (byte)255 : (byte)0;
                        ptr += 3;
                    }
                    ptr += offset;
                }
                newBmp.UnlockBits(newData);
                return newBmp;
            }
        }

        /// <summary>
        /// OTSU自动阈值法二值化
        /// </summary>
        public static Bitmap OtsuThreshold(this Bitmap bmp)
        {
            int width = bmp.Width, height = bmp.Height;
            byte threshold = 0;
            int[] hist = new int[256];

            BitmapData data = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            unsafe
            {
                byte* p = (byte*)data.Scan0;
                int offset = data.Stride - width * 4;
                for (int j = 0; j < height; j++)
                {
                    for (int i = 0; i < width; i++)
                    {
                        hist[p[0]]++;
                        p += 4;
                    }
                    p += offset;
                }
                bmp.UnlockBits(data);
            }

            double allSum = 0, smallSum = 0;
            int allPixelNumber = 0, smallPixelNumber = 0;
            //计算灰度为I的像素出现的概率
            for (int i = 0; i < 256; i++)
            {
                allSum += i * hist[i];
                allPixelNumber += hist[i];
            }
            double maxValue = -1.0;
            for (int i = 0; i < 256; i++)
            {
                smallPixelNumber += hist[i];
                int bigPixelNumber = allPixelNumber - smallPixelNumber;
                if (bigPixelNumber == 0)
                {
                    break;
                }
                smallSum += i * hist[i];
                double bigSum = allSum - smallSum;
                double smallProbability = smallSum / smallPixelNumber;
                double bigProbability = bigSum / bigPixelNumber;
                double probability = smallPixelNumber * smallProbability + bigPixelNumber * bigProbability * bigProbability;
                if (probability > maxValue)
                {
                    maxValue = probability;
                    threshold = (byte)i;
                }
            }
            return Threshoding(bmp, threshold);
        }

        /// <summary>
        /// 固定阈值的二值化
        /// </summary>
        /// <param name="bmp">待处理的图片</param>
        /// <param name="threshold">灰度阈值</param>
        /// <returns> </returns>
        public static Bitmap Threshoding(this Bitmap bmp, byte threshold)
        {
            int width = bmp.Width, height = bmp.Height;
            Bitmap newBmp = bmp.Clone(new Rectangle(0, 0, bmp.Width, bmp.Height), bmp.PixelFormat);
            BitmapData data = newBmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            unsafe
            {
                byte* ptr = (byte*)data.Scan0;
                int offset = data.Stride - width * 4;
                for (int j = 0; j < height; j++)
                {
                    for (int i = 0; i < width; i++)
                    {
                        byte gray = (byte)((ptr[2] + ptr[1] + ptr[0]) / 3);
                        if (gray >= threshold)
                        {
                            ptr[0] = ptr[1] = ptr[2] = 255;
                        }
                        else
                        {
                            ptr[0] = ptr[1] = ptr[2] = 0;
                        }
                        ptr += 4;
                    }
                    ptr += offset;
                }
                newBmp.UnlockBits(data);
                return newBmp;
            }
        }

        /// <summary>
        /// 获取有效图形并调整为可平均分割的大小
        /// </summary>
        /// <param name="bmp">待处理的图片</param>
        /// <param name="gray">灰度阈值</param>
        /// <param name="charCount">字符数量</param>
        /// <returns></returns>
        public static Bitmap ToValid(this Bitmap bmp, byte gray, int charCount)
        {
            int posx1 = bmp.Width;
            int posy1 = bmp.Height;
            int posx2 = 0;
            int posy2 = 0;
            for (int i = 0; i < bmp.Height; i++) //找有效区
            {
                for (int j = 0; j < bmp.Width; j++)
                {
                    int pixelValue = bmp.GetPixel(j, i).R;
                    if (pixelValue < gray) //根据灰度值
                    {
                        if (posx1 > j)
                        {
                            posx1 = j;
                        }
                        if (posy1 > i)
                        {
                            posy1 = i;
                        }

                        if (posx2 < j)
                        {
                            posx2 = j;
                        }
                        if (posy2 < i)
                        {
                            posy2 = i;
                        }
                    }
                }
            }
            // 确保能整除
            int span = charCount - (posx2 - posx1 + 1) % charCount; //可整除的差额数
            if (span < charCount)
            {
                int leftSpan = span / 2; //分配到左边的空列 ，如span为单数,则右边比左边大1
                if (posx1 > leftSpan)
                {
                    posx1 = posx1 - leftSpan;
                }
                if (posx2 + span - leftSpan < bmp.Width)
                {
                    posx2 = posx2 + span - leftSpan;
                }
            }
            //复制新图
            Rectangle cloneRect = new Rectangle(posx1, posy1, posx2 - posx1 + 1, posy2 - posy1 + 1);
            Bitmap newBmp = bmp.Clone(cloneRect, bmp.PixelFormat);
            return newBmp;
        }

        /// <summary>
        /// 去除空白边界获取有效的图形
        /// </summary>
        /// <param name="bmp">待处理的图片</param>
        /// <param name="gray">灰度阈值</param>
        /// <returns></returns>
        public static Bitmap ToValid(this Bitmap bmp, byte gray)
        {
            int posx1 = bmp.Width;
            int posy1 = bmp.Height;
            int posx2 = 0;
            int posy2 = 0;
            for (int y = 0; y < bmp.Height; y++) //找有效区
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    int pixelValue = bmp.GetPixel(x, y).R;
                    if (pixelValue < gray) //根据灰度值
                    {
                        if (posx1 > x)
                        {
                            posx1 = x;
                        }
                        if (posy1 > y)
                        {
                            posy1 = y;
                        }

                        if (posx2 < x)
                        {
                            posx2 = x;
                        }
                        if (posy2 < y)
                        {
                            posy2 = y;
                        }
                    }
                }
            }
            //复制新图
            Rectangle cloneRect = new Rectangle(posx1, posy1, posx2 - posx1 + 1, posy2 - posy1 + 1);
            return bmp.Clone(cloneRect, bmp.PixelFormat);
        }

        /// <summary>
        /// 平均分割图片
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="rowNum">水平上分割数</param>
        /// <param name="colNum">垂直上分割数</param>
        /// <returns>分割好的图片数组</returns>
        public static Bitmap[] SplitAverage(this Bitmap bmp, int rowNum, int colNum)
        {
            if (rowNum == 0 || colNum == 0)
            {
                return null;
            }
            int singW = bmp.Width / rowNum;
            int singH = bmp.Height / colNum;
            Bitmap[] picArray = new Bitmap[rowNum * colNum];

            for (int i = 0; i < colNum; i++)
            {
                for (int j = 0; j < rowNum; j++)
                {
                    Rectangle cloneRect = new Rectangle(j * singW, i * singH, singW, singH);
                    picArray[i * rowNum + j] = bmp.Clone(cloneRect, bmp.PixelFormat); //复制小块图
                }
            }
            return picArray;
        }

        /// <summary>
        /// 获取灰度图片的点阵描述字符串，1表示黑点，0表示空白
        /// </summary>
        /// <param name="bmp">待处理的图片</param>
        /// <param name="gray">灰度临界值</param>
        /// <param name="lineBreak">是否换行，默认false</param>
        /// <returns></returns>
        public static string ToCodeString(this Bitmap bmp, byte gray, bool lineBreak = false)
        {
            string code = string.Empty;
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    Color piexl = bmp.GetPixel(x, y);
                    if (piexl.R < gray)
                    {
                        code += "1";
                    }
                    else
                    {
                        code += "0";
                    }
                }
                if (lineBreak)
                {
                    code += "\r\n";
                }
            }
            return code;
        }

        private static byte GetAverageColor(byte[,] source, int x, int y, int w, int h)
        {
            int result = source[x, y]
                + (x == 0 ? 255 : source[x - 1, y])
                + (x == 0 || y == 0 ? 255 : source[x - 1, y - 1])
                + (x == 0 || y == h - 1 ? 255 : source[x - 1, y + 1])
                + (y == 0 ? 255 : source[x, y - 1])
                + (y == h - 1 ? 255 : source[x, y + 1])
                + (x == w - 1 ? 255 : source[x + 1, y])
                + (x == w - 1 || y == 0 ? 255 : source[x + 1, y - 1])
                + (x == w - 1 || y == h - 1 ? 255 : source[x + 1, y + 1]);
            return (byte)(result / 9);
        }

        private static byte GetGrayValue(Color pixel)
        {
            return GetGrayValue(pixel.R, pixel.G, pixel.B);
        }

        private static byte GetGrayValue(byte red, byte green, byte blue)
        {
            return (byte)((red * 19595 + green * 38469 + blue * 7472) >> 16);
        }

        private static bool IsBlack(byte value)
        {
            return value == 0;
        }

        private static bool IsWhite(byte value)
        {
            return value == 255;
        }

        #endregion
    }
}