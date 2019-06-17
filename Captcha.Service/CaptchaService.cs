using Captcha.Dto;
using Captcha.Service.Contract;
using Captcha.Util;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Captcha.Service
{
    public class CaptchaService : ICaptchaService
    {
        #region Private Fields

        private readonly IMemoryCache _cache;
        private readonly IHostingEnvironment _hostingEnvironment;

        #endregion

        #region Constructors

        public CaptchaService(IMemoryCache cache, IHostingEnvironment hostingEnvironment)
        {
            _cache = cache;
            _hostingEnvironment = hostingEnvironment;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 获取图片验证码
        /// </summary>
        /// <param name="imgCaptchaDto">图形验证码请求信息</param>
        /// <returns></returns>
        public CaptchaResult GetImageCaptcha(ImgCaptchaDto imgCaptchaDto)
        {
            var captchaCode = ImageCaptchaHelper.GenerateCaptchaCode();
            var result = ImageCaptchaHelper.GenerateCaptcha(100, 36, captchaCode);
            _cache.Set($"ImgCaptcha{imgCaptchaDto.ImgCaptchaType}{imgCaptchaDto.Mobile}", result.CaptchaCode);

            return result;
        }

        /// <summary>
        /// 验证图片验证码
        /// </summary>
        /// <param name="imgCaptchaDto">图形验证码信息</param>
        /// <returns></returns>
        public bool ValidateImageCaptcha(ImgCaptchaDto imgCaptchaDto)
        {
            var cachedImageCaptcha = _cache.Get<string>($"ImgCaptcha{imgCaptchaDto.ImgCaptchaType}{imgCaptchaDto.Mobile}");
            if (string.Equals(imgCaptchaDto.ImgCaptcha, cachedImageCaptcha, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取短信验证码
        /// </summary>
        /// <param name="msgCaptchaDto">短信验证码请求信息</param>
        /// <returns></returns>
        public (bool, string) GetMsgCaptcha(MsgCaptchaDto msgCaptchaDto)
        {
            if (string.IsNullOrWhiteSpace(msgCaptchaDto.ImgCaptcha))
            {
                throw new BusinessException((int)ErrorCode.BadRequest, "请输入图形验证码");
            }

            var cachedImageCaptcha = _cache.Get<string>($"ImgCaptcha{msgCaptchaDto.MsgCaptchaType}{msgCaptchaDto.Mobile}");
            if (!string.Equals(msgCaptchaDto.ImgCaptcha, cachedImageCaptcha, StringComparison.OrdinalIgnoreCase))
            {
                return (false, "验证失败，请输入正确手机号及获取到的图形验证码");
            }

            string key = $"MsgCaptcha{msgCaptchaDto.MsgCaptchaType}{msgCaptchaDto.Mobile}";
            var cachedMsgCaptcha = _cache.Get<MsgCaptchaDto>(key);
            if (cachedMsgCaptcha != null)
            {
                var offsetSecionds = (DateTime.Now - cachedMsgCaptcha.CreateTime).Seconds;
                if (offsetSecionds < 60)
                {
                    return (false, $"短信验证码获取太频繁，请{60 - offsetSecionds}秒之后再获取");
                }
            }

            var msgCaptcha = MsgCaptchaHelper.CreateRandomNumber(6);
            msgCaptchaDto.MsgCaptcha = msgCaptcha;
            msgCaptchaDto.CreateTime = DateTime.Now;
            msgCaptchaDto.ValidateCount = 0;
            _cache.Set(key, msgCaptchaDto, TimeSpan.FromMinutes(2));

            if (_hostingEnvironment.IsProduction())
            {
                //TODO：调用第三方SDK实际发送短信
                return (true, "发送成功");
            }
            else        //非生产环境，直接将验证码返给前端，便于调查跟踪
            {
                return (true, $"发送成功，短信验证码为：{msgCaptcha}");
            }
        }

        /// <summary>
        /// 验证短信验证码
        /// </summary>
        /// <param name="msgCaptchaDto">短信验证码信息</param>
        /// <returns></returns>
        public (bool, string) ValidateMsgCaptcha(MsgCaptchaDto msgCaptchaDto)
        {
            var key = $"MsgCaptcha{msgCaptchaDto.MsgCaptchaType}{msgCaptchaDto.Mobile}";
            var cachedMsgCaptcha = _cache.Get<MsgCaptchaDto>(key);
            if (cachedMsgCaptcha == null)
            {
                return (false, "短信验证码无效，请重新获取");
            }

            if (cachedMsgCaptcha.ValidateCount >= 3)
            {
                _cache.Remove(key);
                return (false, "短信验证码已失效，请重新获取");
            }
            cachedMsgCaptcha.ValidateCount++;

            if (!string.Equals(cachedMsgCaptcha.MsgCaptcha, msgCaptchaDto.MsgCaptcha, StringComparison.OrdinalIgnoreCase))
            {
                return (false, "短信验证码错误");
            }
            else
            {
                return (true, "验证通过");
            }
        }

        #endregion
    }
}
