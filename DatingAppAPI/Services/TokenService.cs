using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DatingAppAPI.Entities;
using DatingAppAPI.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace DatingAppAPI.Services
{
    public class TokenService : ITokenService
    {
        /*symmetric encrytion is a type in which the same key is used to assign and 
        verify the token i.e the same key is used to encrypt and decrypt the electronic information.
        */
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config){
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }
        public string CreateToken(AppUser user)
        {
            //1. Identify the claims that you want put as a part of your token.
            var claims = new List<Claim>{
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
            };

            //2. Create signing credentials.
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            /*3. Create token descriptor that shows what will be inside your token and how will it look.
                 it includes subject that contains claims, expires on which date, signing credentails to sign the token.*/
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            //4. Create token handler.
            var tokenHandler = new JwtSecurityTokenHandler();

            //5. Create token through token handler.
            var token = tokenHandler.CreateToken(tokenDescriptor);

            //6. return written token through token handler.
            return tokenHandler.WriteToken(token);
        }
    }
}