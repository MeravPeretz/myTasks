using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace myTasks.Interfaces;

public interface ITokenService
{
    SecurityToken GetToken(List<Claim> claims);
    TokenValidationParameters GetTokenValidationParameters() ;   
    string WriteToken(SecurityToken token); 
}
