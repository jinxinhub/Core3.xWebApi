using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Core3.xWebApi.Controllers
{
    public class GetTokenController : Controller
    {
        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult<string>> GetToken(string name, string pwd)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pwd))
            {
                return new JsonResult(new
                {
                    Status = false,
                    message = "用户名或密码不能为空"
                });
            }
            TokenModelJWT tokenModel = new TokenModelJWT();
            tokenModel.Uid = 1;
            tokenModel.Role = name;

            var jwtStr = JwtHelper.IssueJWT(tokenModel);
            var suc = true;
            return Ok(new
            {
                success = suc,
                token = jwtStr
            });
        }
    }
}