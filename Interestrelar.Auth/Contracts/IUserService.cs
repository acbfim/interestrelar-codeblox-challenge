using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interestrelar.Domain;

namespace Interestrelar.Auth.Contracts;

public interface IUserService
{
    Task<RetornoDto> RefreshToken(TokenDto tokenDto);
    
}
