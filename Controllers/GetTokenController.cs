using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Core3.xWebApi.Controllers
{
    [ApiController]
    public class GetTokenController : ControllerBase
    {
        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetToken/GetNewToken")]
        public async Task<ActionResult<string>> GetNewToken(string name, string pwd)
        {
            bool suc = false;
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pwd))
            {
                return new JsonResult(new
                {
                    Status = false,
                    message = "用户名或密码不能为空"
                });
            }
            else
            {
                TokenModelJWT tokenModel = new TokenModelJWT();
                tokenModel.Uid = 1;
                tokenModel.Role = name;

                var jwStr = JwtHelper.IssueJWT(tokenModel);
                suc = true;

                return Ok(
                    new { success = suc, token = jwStr });
            }
        }
    }
}
