using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Extension
{
    /// <summary>
    /// 类型转换
    /// </summary>
    public static class ConvertEx
    {
        #region string 

        /// <summary>
        /// 将string类型的数据转成Short类型
        /// </summary>
        /// <param name="value">数据</param>
        /// <param name="output">默认输出</param>
        /// <returns></returns>
        public static short ToShort(this string value, short output)
        {
            try
            {
                return short.Parse(value);
            }
            catch (System.Exception)
            {
                return output;
            }
        }

        /// <summary>
        /// 将string类型的数据转成Short类型
        /// </summary>
        /// <param name="value">数据</param>
        /// <param name="output">默认输出</param>
        /// <returns></returns>
        public static short? ToShort(this string value, short? output)
        {
            try
            {
                return short.Parse(value);
            }
            catch (System.Exception)
            {
                return output;
            }
        }

        /// <summary>
        /// 将string类型的数据转成Int类型
        /// </summary>
        /// <param name="value">数据</param>
        /// <param name="output">默认输出</param>
        /// <returns></returns>
        public static int ToInt(this string value, int output)
        {
            try
            {
                return int.Parse(value);
            }
            catch (System.Exception)
            {
                return output;
            }
        }

        /// <summary>
        /// 将string类型的数据转成Int类型
        /// </summary>
        /// <param name="value">数据</param>
        /// <param name="output">默认输出</param>
        /// <returns></returns>
        public static int? ToInt(this string value, int? output)
        {
            try
            {
                return int.Parse(value);
            }
            catch (System.Exception)
            {
                return output;
            }
        }

        /// <summary>
        /// 将string类型的数据转成Long类型
        /// </summary>
        /// <param name="value">数据</param>
        /// <param name="output">默认输出</param>
        /// <returns></returns>
        public static long ToLong(this string value, long output)
        {
            try
            {
                return long.Parse(value);
            }
            catch (System.Exception)
            {
                return output;
            }
        }

        /// <summary>
        /// 将string类型的数据转成Long类型
        /// </summary>
        /// <param name="value">数据</param>
        /// <param name="output">默认输出</param>
        /// <returns></returns>
        public static long? ToLong(this string value, long? output)
        {
            try
            {
                return int.Parse(value);
            }
            catch (System.Exception)
            {
                return output;
            }
        }

        /// <summary>
        /// 将string类型的数据转成float类型
        /// </summary>
        /// <param name="value">数据</param>
        /// <param name="output">默认输出</param>
        /// <returns></returns>
        public static float ToFloat(this string value, float output)
        {
            try
            {
                return float.Parse(value);
            }
            catch (System.Exception)
            {
                return output;
            }
        }

        /// <summary>
        /// 将string类型的数据转成float类型
        /// </summary>
        /// <param name="value">数据</param>
        /// <param name="output">默认输出</param>
        /// <returns></returns>
        public static float? ToFloat(this string value, float? output)
        {
            try
            {
                return float.Parse(value);
            }
            catch (System.Exception)
            {
                return output;
            }
        }

        /// <summary>
        /// 将string类型的数据转成Double类型
        /// </summary>
        /// <param name="value">数据</param>
        /// <param name="output">默认输出</param>
        /// <returns></returns>
        public static double ToDouble(this string value, double output)
        {
            try
            {
                return double.Parse(value);
            }
            catch (System.Exception)
            {
                return output;
            }
        }

        /// <summary>
        /// 将string类型的数据转成Double类型
        /// </summary>
        /// <param name="value">数据</param>
        /// <param name="output">默认输出</param>
        /// <returns></returns>
        public static double? ToDouble(this string value, double? output)
        {
            try
            {
                return double.Parse(value);
            }
            catch (System.Exception)
            {
                return output;
            }
        }

        /// <summary>
        /// 将string类型的数据转成Decimal类型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string value, decimal output)
        {
            try
            {
                return decimal.Parse(value);
            }
            catch (System.Exception)
            {
                return output;
            }
        }

        /// <summary>
        /// 将string类型的数据转成Decimal类型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public static decimal? ToDecimal(this string value, decimal? output)
        {
            try
            {
                return decimal.Parse(value);
            }
            catch (System.Exception)
            {
                return output;
            }
        }

        /// <summary>
        /// 将string类型的数据转成DateTime类型
        /// </summary>
        /// <param name="value">数据</param>
        /// <param name="output">错误信息</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string value, DateTime output)
        {
            try
            {
                return DateTime.Parse(value);
            }
            catch (System.Exception)
            {
                return output;
            }
        }

        /// <summary>
        /// 将string类型的数据转成DateTime类型
        /// </summary>
        /// <param name="value">数据</param>
        /// <param name="output">错误信息</param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string value, DateTime? output)
        {
            try
            {
                return DateTime.Parse(value);
            }
            catch (System.Exception)
            {
                return output;
            }
        }

        /// <summary>
        /// 将string类型的数据转成byte[]类型
        /// utf8
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToByte(this string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        /// <summary>
        /// 将string类型的数据转成byte[]类型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encode">编码类型</param>
        /// <returns></returns>
        public static byte[] ToByte(this string value, Encoding encode)
        {
            return encode.GetBytes(value);
        }

        /// <summary>
        /// 转化为bool值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public static bool ToBool(this string value, bool output)
        {
            try
            {
                return bool.Parse(value);
            }
            catch (System.Exception)
            {
                return output;
            }
        }

        /// <summary>
        /// 转化为指定字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public static string ToString(this string value, string output)
        {
            if (string.IsNullOrWhiteSpace(value))
                return output;
            return value;
        }

        #endregion

        #region byte[]

        /// <summary>
        /// 将字节数组转化成字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToString(this byte[] value)
        {
            return Encoding.UTF8.GetString(value);
        }

        /// <summary>
        /// 将字节数组转化成字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encode">编码方式</param>
        /// <returns></returns>
        public static string ToString(this byte[] value, Encoding encode)
        {
            return encode.GetString(value);
        }

        #endregion

        #region Stream

        public static async Task<string> GetStringAsync(this Stream stream)
        {
            var reader = new StreamReader(stream);
            var result = await reader.ReadToEndAsync();
            reader.Dispose();
            stream.Dispose();
            return result;
        }

        public static string GetString(this Stream stream)
        {
            var reader = new StreamReader(stream);
            var result = reader.ReadToEnd();
            reader.Dispose();
            stream.Dispose();
            return result;
        }

        public static Stream ToStream(this string str)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(str);
            writer.Flush();
            writer.Dispose();
            stream.Dispose();
            return stream;
        }

        #endregion

        #region Enum

        /// <summary>
        /// 转化为枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        /// <summary>
        /// 转化为枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this string value, T code)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), value);
            }
            catch (System.Exception)
            {
                return code;
            }
        }


        /// <summary>  
        /// 获取枚举描述
        /// </summary>  
        /// <param name="en">枚举</param>  
        /// <returns>返回枚举的描述 </returns>  
        public static string GetDescription(this Enum en)
        {
            var type = en.GetType(); //获取类型  
            var memberInfos = type.GetMember(en.ToString()); //获取成员  
            if (memberInfos != null && memberInfos.Length > 0)
            {
                var attrs =
                    memberInfos[0]
                        .GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[]; //获取描述特性  
                if (attrs != null && attrs.Length > 0)
                {
                    return attrs[0].Description; //返回当前描述
                }
            }

            return en.ToString();
        }

        #endregion
    }
}
