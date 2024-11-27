using E_CommerceDomain.Entities.Account_Module;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceRepository.Helpers
{
    public class AccountHelper
    {
        private readonly IConfiguration Config;
        private readonly UserManager<ApplicationUser> UserManager;

        public AccountHelper(IConfiguration Config, UserManager<ApplicationUser> UserManager)
        {
            this.Config = Config;
            this.UserManager = UserManager;
        }
        public async Task<string> GenerateJwt(ApplicationUser User)
        {
            // Generate The Jwt Design


            // Custom Claims
            List<Claim> Claims = new List<Claim>();
            Claims.Add(new Claim(ClaimTypes.NameIdentifier, User.Id));
            Claims.Add(new Claim(ClaimTypes.Email, User.Email));
            var Roles = await UserManager.GetRolesAsync(User);
            foreach(var RoleName in Roles)
            {
                Claims.Add(new Claim(ClaimTypes.Role, RoleName));
            }

            // SymmetricKey
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config["Jwt:SecretKey"]));

            // SymmetricKey & Algorithms
            SigningCredentials Credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // The Design Of Token
            var TokenDesign = new JwtSecurityToken(

                issuer: Config["Jwt:ProviderUrl"],
                audience: Config["Jwt:ConsumerUrl"],
                expires: DateTime.Now.AddDays(int.Parse(Config["Jwt:ExpireHours"])),
                claims: Claims,
                signingCredentials: Credential

                );

            // Using Handler To Generate The Token 
            var Token = new JwtSecurityTokenHandler().WriteToken(TokenDesign);

            // Return The Token
            return Token;
        }
    }
}
