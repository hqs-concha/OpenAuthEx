using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Tools.Exception;

namespace Tools.Extension
{
    /// <summary>
    /// 正则扩展
    /// </summary>
    public static class RegEx
    {
        #region 正则通用方法

        /// <summary>
        /// 匹配 
        /// 正则表达式进行验证
        /// </summary>
        /// <param name="value"></param>
        /// <param name="formula">正则表达式</param>
        /// <returns></returns>
        public static bool Match(this string value, string formula)
        {
            Regex reg = new Regex(formula);
            if (reg.IsMatch(value))
                return true;
            return false;
        }

        /// <summary>
        /// 匹配 
        /// 正则表达式进行验证
        /// </summary>
        /// <param name="value"></param>
        /// <param name="formula">正则表达式</param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public static string Match(this string value, string formula, string errorMsg)
        {
            Regex reg = new Regex(formula);
            if (reg.IsMatch(value))
                return value;
            throw new CustomException(errorMsg);
        }

        /// <summary>
        /// 切割     
        /// 分割字符串返回string[]的对象
        /// </summary>
        /// <param name="value"></param>
        /// <param name="formula">分隔符</param>
        /// <returns></returns>
        public static string[] Split(this string value, string formula)
        {
            string[] arr = Regex.Split(value, formula);
            return arr;
        }

        /// <summary>
        /// 替换
        /// 替换符合正则表达式的字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="formula">正则表达式</param>
        /// <param name="aimText">替换后的字符串</param>
        /// <returns></returns>
        public static string Replace(this string value, string formula, string aimText)
        {
            return Regex.Replace(value, formula, aimText);
        }

        /// <summary>
        ///  获取
        ///  获取符合正则表达式的字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="formula"></param>
        /// <returns></returns>
        public static string[] Get(this string value, string formula)
        {
            string[] strReturn = new string[Regex.Matches(value, formula).Count];
            int i = 0;
            foreach (Match mch in Regex.Matches(value, formula))
            {
                strReturn[i] += mch.Value.Trim();
                i++;
            }
            return strReturn;
        }

        #endregion

        #region 常用验证

        /// <summary>
        /// 是否是金额
        /// 两位小数的正实数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsMoney(string value)
        {
            return Match(value, Formula.Money);
        }

        /// <summary>
        /// 是否是金额
        /// 两位小数的正实数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public static string IsMoney(this string value, string errorMsg)
        {
            if (IsMoney(value))
                return value;
            throw new CustomException(errorMsg);
        }

        /// <summary>
        /// 是否是手机号
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsPhone(string value)
        {
            return Match(value, Formula.Phone);
        }

        /// <summary>
        /// 是否是手机号
        /// </summary>
        /// <param name="value"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public static string IsPhone(this string value, string errorMsg)
        {
            if (IsPhone(value))
                return value;
            throw new CustomException(errorMsg);
        }

        /// <summary>
        /// 是否是邮箱
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEmail(string value)
        {
            return Match(value, Formula.Email);
        }

        /// <summary>
        /// 是否是邮箱
        /// </summary>
        /// <param name="value"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public static string IsEmail(this string value, string errorMsg)
        {
            if (IsEmail(value))
                return value;
            throw new CustomException(errorMsg);
        }

        /// <summary>
        /// 是否是电话
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsTel(string value)
        {
            return Match(value, Formula.Tel);
        }

        /// <summary>
        /// 是否是电话
        /// </summary>
        /// <param name="value"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public static string IsTel(this string value, string errorMsg)
        {
            if (IsTel(value))
                return value;
            throw new CustomException(errorMsg);
        }

        /// <summary>
        /// 是否是身份证
        /// 包含格式，省份，年月日，尾数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsIdCard(string value)
        {
            var flag = Match(value, Formula.IdCard);
            if (flag == false) return false;
            var provinceId = value.Substring(0, 2);
            flag = IsValidProvinceId(provinceId);
            if (flag == false) return false;
            var date = value.Substring(6, 8);
            flag = IsValidDate(date);
            if (flag == false) return false;
            flag = SumPower(value);
            return flag;
        }

        /// <summary>
        /// 是否是身份证
        /// 包含格式，省份，年月日，尾数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public static string IsIdCard(this string value, string errorMsg)
        {
            if (IsIdCard(value))
                return value;
            throw new CustomException(errorMsg);
        }

        #region 身份证格式验证

        private static readonly int[] Power = { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
        private static readonly string[] RefNumber = { "1", "0", "X", "9", "8", "7", "6", "5", "4", "3", "2" };
        private static readonly string[] ProvinceCode = { "11", "12", "13", "14", "15", "21", "22", "23", "31", "32", "33", "34", "35", "36", "37", "41", "42", "43", "44", "45", "46", "50", "51", "52", "53", "54", "61", "62", "63", "64", "65", "71", "81", "82", "91" };

        private static bool SumPower(string cardId)
        {
            char[] c = cardId.ToArray();
            int result = 0;
            for (int i = 0; i < Power.Length; i++)
            {
                result += Power[i] * int.Parse(c[i].ToString());
            }
            return RefNumber[result % 11] == c[17].ToString().ToUpper();
        }

        private static bool IsValidDate(string date)
        {
            if (date == null)
            {
                return false;
            }
            try
            {
                var dt = DateTime.ParseExact(date, "yyyyMMdd", CultureInfo.CurrentCulture);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool IsValidProvinceId(string provinceId)
        {
            return ProvinceCode.Any(p => p == provinceId);
        }

        #endregion

        #endregion

        /// <summary>
        /// 常用正则表达式
        /// </summary>
        public static class Formula
        {
            /// <summary>
            /// 手机号
            /// </summary>
            public const string Phone = @"^1[0-9]{10}$";

            /// <summary>
            /// 邮箱
            /// </summary>
            public const string Email = @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

            /// <summary>
            /// 电话
            /// </summary>
            public const string Tel = @"^(\d{3,4}-)\d{7,8}$";

            /// <summary>
            /// 身份证
            /// </summary>
            public const string IdCard = @"^[1-9]\d{5}(18|19|([23]\d))\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{3}[0-9Xx]$";

            /// <summary>
            /// 金额
            /// </summary>
            public const string Money = @"^[0-9]+(\.[0-9]{2})?$";

        }
    }
}
