using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Captcha.Dto
{
    /// <summary>
    /// 图形验证码
    /// </summary>
    public class ImgCaptchaDto
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
        /// 图形验证码类型
        /// </summary>
        [Required(ErrorMessage = "请提供图形验证码类型化")]
        public int ImgCaptchaType
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
    }
}
