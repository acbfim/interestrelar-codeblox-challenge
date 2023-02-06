using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Interestrelar.Auth.Contracts;
using Interestrelar.Auth.Security;
using Interestrelar.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


namespace Interestrelar.Auth.Application;

public class UserService : IUserService
{


    public async Task<RetornoDto> RefreshToken(TokenDto tokenDto)
    {
        var retorno = new RetornoDto();

        retorno.StatusCode = StatusCodes.Status404NotFound;
        retorno.Success = false;

        if (tokenDto is null)
        {
            retorno.Message = "Token null";
            return retorno;
        }

        string accessToken = tokenDto.AccessToken;
        string refreshToken = tokenDto.RefreshToken;


        try
        {
            var principal = await TokenService.GetPrincipalFromExpiredToken(refreshToken);

            var username = principal.Identity.Name;

            var newAccessToken = TokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = TokenService.GenerateRefreshToken(principal.Claims);


            var newToken = new TokenDto()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };

            retorno.Success = true;
            retorno.StatusCode = 200;
            retorno.Data = newToken;
            retorno.Message = "Access token gerado com sucesso";

           
        }catch (SecurityTokenException ex)
        {
            Console.WriteLine(ex.StackTrace);
            retorno.StatusCode = StatusCodes.Status403Forbidden;
            retorno.Success = false;
            retorno.Message = "Erro ao tentar realizar atualizar o token. Token invalido";
            retorno.Data = ex.Message;
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
            retorno.StatusCode = StatusCodes.Status500InternalServerError;
            retorno.Success = false;
            retorno.Message = "Erro ao tentar realizar atualizar o token";
            retorno.Data = ex.Message;
        }


        return retorno;
    }
}
