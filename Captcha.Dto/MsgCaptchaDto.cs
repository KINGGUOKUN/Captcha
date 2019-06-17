using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Captcha.Dto
{
    /// <summary>
    /// 短信验证码
    /// </summary>
    public class MsgCaptchaDto
    {
        /// <summary>
        /// 手机号
        /// </summary>
        [Required(ErrorMessage = "请输入手机号")]
        public string Mobile
        {
            get;
            set;
        }

        /// <summary>
        /// 图形验证码
        /// </summary>
        public string ImgCaptcha
        {
            get;
            set;
        }

        /// <summary>
        /// 短信验证码类型
        /// </summary>
        [Required(ErrorMessage = "请提供短信验证码类型")]
        public int MsgCaptchaType
        {
            get;
            set;
        }

        /// <summary>
        /// 短信验证码
        /// </summary>
        public string MsgCaptcha
        {
            get;
            set;
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get;
            set;
        }

        /// <summary>
        /// 校验次数
        /// </summary>
        public int ValidateCount
        {
            get;
            set;
        }
    }
}
