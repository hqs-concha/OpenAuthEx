using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;

namespace Tools.Utils
{
    /// <summary>
    /// 图形验证码辅助类
    /// </summary>
    public class CaptchaHelper
    {
        private const double PI2 = 6.283185307179586476925286766559;

        #region Utilities

        /// <summary>
        /// 产生波形滤镜效果
        /// 正弦曲线Wave扭曲图片（Edit By 51aspx.com）
        /// </summary>
        /// <param name="bitmap">图片路径</param>
        /// <param name="twisted">如果扭曲则选择为True</param>
        /// <param name="amplitude">波形的幅度倍数，越大扭曲的程度越高，一般为3</param>
        /// <param name="phase">波形的起始相位，取值区间[0-2*PI)</param>
        /// <returns></returns>
        private static Bitmap TwistImage(Bitmap bitmap, bool twisted, double amplitude, double phase)
        {
            var lDestBmp = new Bitmap(bitmap.Width, bitmap.Height);
            // 将位图背景填充为白色
            var lGraph = Graphics.FromImage(lDestBmp);
            try
            {
                lGraph.FillRectangle(new SolidBrush(Color.White), 0, 0, lDestBmp.Width, lDestBmp.Height);
            }
            finally
            {
                lGraph.Dispose();
            }
            var lDBaseAxisLen = twisted ? (double)lDestBmp.Height : (double)lDestBmp.Width;
            for (var lI = 0; lI < lDestBmp.Width; lI++)
            {
                for (var lJ = 0; lJ < lDestBmp.Height; lJ++)
                {
                    double dx = 0;
                    dx = twisted ? (PI2 * (double)lJ) / lDBaseAxisLen : (PI2 * (double)lI) / lDBaseAxisLen;
                    dx += phase;
                    var lDy = Math.Sin(dx);
                    // 取得当前点的颜色
                    int lNOldX = 0, lNOldY = 0;
                    lNOldX = twisted ? lI + (int)(lDy * amplitude) : lI;
                    lNOldY = twisted ? lJ : lJ + (int)(lDy * amplitude);
                    var lColor = bitmap.GetPixel(lI, lJ);
                    if (lNOldX >= 0 && lNOldX < lDestBmp.Width
                        && lNOldY >= 0 && lNOldY < lDestBmp.Height)
                    {
                        lDestBmp.SetPixel(lNOldX, lNOldY, lColor);
                    }
                }
            }
            return lDestBmp;
        }

        #endregion

        /// <summary>
        /// 产生数字型的验证码，缺省六位数
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public string CreateRandomDigitCode(int length = 6)
        {
            var lCodeLen = (length >= 1) ? length : 1;

            var lRandom = new Random(unchecked((int)DateTime.Now.Ticks));
            var lCode = string.Empty;
            for (var i = 0; i < lCodeLen; i++)
                lCode = string.Concat(lCode, lRandom.Next(10).ToString(CultureInfo.InvariantCulture));
            return lCode;
        }

        /// <summary>
        /// 产生随机的字符验证码，包含数字和字母
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public string CreateVerifyCode(int length = 4)
        {
            var lCodeLen = (length >= 4) ? length : MinLength;

            var lCodeArray = CodeSerial.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var lCode = string.Empty;
            var lRandom = new Random(unchecked((int)DateTime.Now.Ticks));
            for (var i = 0; i < lCodeLen; i++)
            {
                var lRandValue = lRandom.Next(0, lCodeArray.Length - 1);
                lCode = string.Concat(lCode, lCodeArray[lRandValue]);
            }
            return lCode;
        }

        /// <summary>
        /// 生成校验码图片
        /// </summary>
        /// <param name="code">待处理的验证码</param>
        /// <param name="imgWidth">图片宽度</param>
        /// <param name="imgHeight">图片高度</param>
        /// <param name="amplitude">波形的幅度倍数，越大扭曲的程度越高，一般为3</param>
        /// <param name="phase">波形的起始相位，取值区间[0-2*PI)</param>
        /// <returns></returns>
        public byte[] CreateImageCode(string code, int imgWidth, int imgHeight, double amplitude, double phase)
        {
            //const int IMAGE_WIDTH = 94; //(int)(lCode.Length * lFWidth) + 4 + Padding * 2;
            //const int IMAGE_HEIGHT = 34; //lFSize * 2 + Padding;

            var lCode = code;
            var lFSize = FontSize;
            var lFWidth = lFSize + Padding;

            var lBitmap = new Bitmap(imgWidth, imgHeight);
            using (var lGraphics = Graphics.FromImage(lBitmap))
            {
                lGraphics.Clear(BackgroundColor);
                var lRand = new Random();

                // 给背景添加随机生成的燥点
                if (ChaosEnabled)
                {
                    var lPen = new Pen(ChaosColor, 0);
                    var lC = MinLength * 10;
                    for (var lI = 0; lI < lC; lI++)
                    {
                        var lX = lRand.Next(lBitmap.Width);
                        var lY = lRand.Next(lBitmap.Height);
                        lGraphics.DrawRectangle(lPen, lX, lY, 1, 1);
                    }
                }

                // 随机字体和颜色的验证码字符
                var lHeight = (imgHeight - FontSize - Padding * 2) / 4;
                var lTop1 = lHeight;
                var lTop2 = lHeight * 2;
                for (var i = 0; i < lCode.Length; i++)
                {
                    var lColorIndex = lRand.Next(Colors.Length - 1);
                    var lFontIndex = lRand.Next(Fonts.Length - 1);
                    var lFont = new Font(Fonts[lFontIndex], lFSize, FontStyle.Bold | FontStyle.Strikeout);
                    var lBrush = new SolidBrush(Colors[lColorIndex]);
                    var lTop = i % 2 == 1 ? lTop2 : lTop1;
                    var lLeft = i * lFWidth;
                    lGraphics.DrawString(lCode.Substring(i, 1), lFont, lBrush, lLeft, lTop);
                }

                // 画一个边框 边框颜色为Color.Gainsboro
                lGraphics.DrawRectangle(new Pen(Color.Gainsboro, 0), 0, 0, lBitmap.Width - 1, lBitmap.Height - 1);
            }

            // 产生波形
            lBitmap = TwistImage(lBitmap, true, amplitude, phase);

            // 将图像保存到指定的流
            using (var lStream = new MemoryStream())
            {
                lBitmap.Save(lStream, ImageFormat.Jpeg);
                return lStream.GetBuffer();
            }
        }

        #region Properties

        /// <summary>
        /// 验证码最小长度（默认为4）
        /// </summary>
        public int MinLength { get; set; } = 4;

        /// <summary>
        /// 验证码字体大小，影响扭曲效果（默认18像素）
        /// </summary>
        public int FontSize { get; set; } = 18;

        /// <summary>
        /// 图片边距(默认2像素)
        /// </summary>
        public int Padding { get; set; } = 2;

        /// <summary>
        /// 是否输出噪点(默认开启)
        /// </summary>
        public bool ChaosEnabled { get; set; } = true;

        /// <summary>
        /// 噪点的颜色(默认为灰色)
        /// </summary>
        public Color ChaosColor { get; set; } = Color.LightGray;

        /// <summary>
        /// 背景色(默认为白色)
        /// </summary>
        public Color BackgroundColor { get; set; } = Color.White;

        /// <summary>
        /// 随机颜色数组（默认为黑、红、深蓝、绿、橙、褐、深青、紫）
        /// </summary>
        public Color[] Colors { get; set; } = {
            Color.Black, Color.Red, Color.DarkBlue, Color.Green,
            Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple
        };

        //string[] mFonts = { "Arial", "Georgia", "微软雅黑" };

        /// <summary>
        /// 随机字体数组（默认为微软雅黑、幼圆）
        /// </summary>
        public string[] Fonts { get; set; } = { "微软雅黑", "幼圆" };

        /// <summary>
        /// 随机码字符串序列，英文半角逗号分隔（默认为阿拉伯数字+英文大小写字母）
        /// </summary>
        public string CodeSerial { get; set; } = "2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,p,q,r," +
                                                 "s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,P,Q,R,S,T,U,V,W,X,Y,Z";

        #endregion
    }
}
