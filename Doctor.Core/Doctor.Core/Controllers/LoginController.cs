using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Doctor.Core.Model;
using Doctor.Core.AuthHelper;

namespace Doctor.Core.Controllers
{
    /// <summary>
    /// 登录管理【无权限】
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class LoginController : Controller
    {
        // readonly ISysUserInfoServices _sysUserInfoServices;

        //public async Task<object> GetJwtStr(string name, string pass)
        //{
        //    string jwtStr = string.Empty;
        //    bool suc = false;

        //    // 获取用户的角色名，请暂时忽略其内部是如何获取的，可以直接用 var userRole="Admin"; 来代替更好理解。
        //    // var userRole = await _sysUserInfoServices.GetUserRoleNameStr(name, pass);
        //    var userRole = "Admin";
        //    if (userRole != null)
        //    {
        //        // 将用户id和角色名，作为单独的自定义变量封装进 token 字符串中。
        //        TokenModelJwt tokenModel = new TokenModelJwt { Uid = 1, Role = userRole };
        //        jwtStr = JwtHelper.IssueJwt(tokenModel);//登录，获取到一定规则的 Token 令牌
        //        suc = true;
        //    }
        //    else
        //    {
        //        jwtStr = "login fail!!!";
        //    }

        //    return Ok(new
        //    {
        //        success = suc,
        //        token = jwtStr
        //    });
        //}

        /// <summary>
        /// 获取JWT的方法4：给 JSONP 测试
        /// </summary>
        /// <param name="callBack"></param>
        /// <param name="id"></param>
        /// <param name="sub"></param>
        /// <param name="expiresSliding"></param>
        /// <param name="expiresAbsoulute"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("jsonp")]
        public void Getjsonp(string callBack, long id = 1, string sub = "Admin", int expiresSliding = 30, int expiresAbsoulute = 30)
        {
            TokenModelJwt tokenModel = new TokenModelJwt
            {
                Uid = id,
                Role = sub
            };

            string jwtStr = JwtHelper.IssueJwt(tokenModel);

            string response = string.Format("\"value\":\"{0}\"", jwtStr);
            string call = callBack + "({" + response + "})";
            Response.WriteAsync(call);
        }

    }
}
