using System;
using System.Collections.Generic;
using System.Text;

namespace Captcha.Util
{
    /// <summary>
    /// 短信验证码工具类
    /// </summary>
    public static class MsgCaptchaHelper
    {
        /// <summary>
        /// 生成指定位数的随机数字码
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string CreateRandomNumber(int length)
        {
            Random random = new Random();
            StringBuilder sbMsgCode = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                sbMsgCode.Append(random.Next(0, 9));
            }

            return sbMsgCode.ToString();
        }
    }
}
