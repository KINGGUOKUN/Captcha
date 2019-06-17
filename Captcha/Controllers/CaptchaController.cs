using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Captcha.Dto;
using Captcha.Service.Contract;
using Captcha.Util;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Captcha.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaptchaController : ControllerBase
    {
        #region Private Fields

        private readonly ICaptchaService _captchaService;

        #endregion

        #region Constructors

        public CaptchaController(ICaptchaService captchaService)
        {
            _captchaService = captchaService;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 获取图片验证码
        /// </summary>
        /// <param name="imgCaptchaDto">图形验证码请求信息</param>
        [HttpGet("img")]
        public IActionResult GetImageCaptcha([FromQuery]ImgCaptchaDto imgCaptchaDto)
        {
            var result = _captchaService.GetImageCaptcha(imgCaptchaDto);
            var stream = new MemoryStream(result.CaptchaByteData);

            return new FileStreamResult(stream, "image/png");
        }

        /// <summary>
        /// 验证图片验证码
        /// </summary>
        /// <param name="imgCaptchaDto">图形验证码信息</param>
        /// <returns></returns>
        [HttpPost("img")]
        public IActionResult ValidateImageCaptcha(ImgCaptchaDto imgCaptchaDto)
        {
            bool isCaptchaValid = _captchaService.ValidateImageCaptcha(imgCaptchaDto);
            if (isCaptchaValid)
            {
                return Ok("图形验证码验证成功");
            }
            else
            {
                return StatusCode(StatusCodes.Status403Forbidden, "验证失败，请输入正确手机号及获取到的验证码");
            }
        }

        /// <summary>
        /// 获取短信验证码
        /// </summary>
        /// <param name="msgCaptchaDto">短信验证码请求信息</param>
        /// <returns></returns>
        [HttpGet("msg")]
        public IActionResult GetMsgCaptcha([FromQuery]MsgCaptchaDto msgCaptchaDto)
        {
            var msgSendResult = _captchaService.GetMsgCaptcha(msgCaptchaDto);
            if (msgSendResult.Item1)
            {
                return Ok(msgSendResult.Item2);
            }
            else
            {
                return StatusCode(StatusCodes.Status403Forbidden, msgSendResult.Item2);
            }
        }

        /// <summary>
        /// 验证短信验证码
        /// </summary>
        /// <param name="msgCaptchaDto">短信验证码信息</param>
        /// <returns></returns>
        [HttpPost("msg")]
        public IActionResult ValidateMsgCaptcha(MsgCaptchaDto msgCaptchaDto)
        {
            var validateResult = _captchaService.ValidateMsgCaptcha(msgCaptchaDto);
            if (validateResult.Item1)
            {
                return Ok(validateResult.Item2);
            }
            else
            {
                return StatusCode(StatusCodes.Status403Forbidden, validateResult.Item2);
            }
        }

        #endregion
    }
}