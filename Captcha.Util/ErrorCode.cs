using System;
using System.Collections.Generic;
using System.Text;

namespace Captcha.Util
{
    /// <summary>
    ///   异常状态码
    /// </summary>
    public enum ErrorCode
    {
        BadRequest = 400,
        Forbidden = 403,
        NotFound = 404,
        Conflict = 409,
        InternalServerError = 500,
        NotImplemented = 501,
    }
}
