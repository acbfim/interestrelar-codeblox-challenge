using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Interestrelar.Auth.Contracts;
using Interestrelar.Auth.Security;
using Interestrelar.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace Interestrelar.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{

public IUserService _userService { get; }

    public UserController(IUserService UserService)
    {
        _userService = UserService;
    }



        /// <summary>
        /// Utilize a palavra "code" para login e senha
        /// </summary>
    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<IActionResult> Login(UserLoginDto user)
    {
        var success = user.UserName == "code" && user.Password == "code";

        if (!success)
            return Unauthorized();

        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName.ToString()),
            };

        var obj = new
        {
            AccessToken = TokenService.GenerateAccessToken(claims)
            ,
            RefreshToken = TokenService.GenerateRefreshToken(claims)
        };

        return Ok(obj);

    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken(TokenDto token)
    {
        var retorno = await this._userService.RefreshToken(token);

        if (retorno.Success)
        {
            return Ok(retorno);
        }
        else
        {
            return this.StatusCode(retorno.StatusCode, retorno);
        }
    }
}
