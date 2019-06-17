using Captcha.Dto;
using Captcha.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace Captcha.Service.Contract
{
    public interface ICaptchaService
    {
        /// <summary>
        /// 获取图片验证码
        /// </summary>
        /// <param name="imgCaptchaDto">图形验证码请求信息</param>
        /// <returns></returns>
        CaptchaResult GetImageCaptcha(ImgCaptchaDto imgCaptchaDto);

        /// <summary>
        /// 验证图片验证码
        /// </summary>
        /// <param name="imgCaptchaDto">图形验证码信息</param>
        /// <returns></returns>
        bool ValidateImageCaptcha(ImgCaptchaDto imgCaptchaDto);

        /// <summary>
        /// 获取短信验证码
        /// </summary>
        /// <param name="msgCaptchaDto">短信验证码请求信息</param>
        /// <returns></returns>
        (bool, string) GetMsgCaptcha(MsgCaptchaDto msgCaptchaDto);

        /// <summary>
        /// 验证短信验证码
        /// </summary>
        /// <param name="msgCaptchaDto">短信验证码信息</param>
        /// <returns></returns>
        (bool, string) ValidateMsgCaptcha(MsgCaptchaDto msgCaptchaDto);
    }
}
