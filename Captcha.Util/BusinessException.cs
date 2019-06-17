using System;
using System.Collections.Generic;
using System.Text;

namespace Captcha.Util
{
    /// <summary>
    /// 自定义业务类型异常
    /// </summary>
    public class BusinessException : Exception
    {
        public BusinessException(int hResult, string message)
            : base(message)
        {
            base.HResult = hResult;
        }
    }
}
