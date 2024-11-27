using E_CommerceDomain.DTOs.Account_Module;
using E_CommerceDomain.Entities.Account_Module;
using E_CommerceDomain.Interfaces.Account_Module;
using E_CommerceRepository.Data.Contexts;
using E_CommerceRepository.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceService.Services.Account_Module
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly IConfiguration Config;

        public AccountService(UserManager<ApplicationUser> UserManager, IConfiguration Config)
        {
            this.UserManager = UserManager;
            this.Config = Config;
        }
        public async Task<List<string>> Register(RegisterDTO User)
        {
            var Errors = new List<string>();

            bool Flag = await CheckUniqueEmail(User.Email);

            if(!Flag)
            {
                Errors.Add("This Email Address Already Token");
                return Errors;
            }

            ApplicationUser UserToDb = new ApplicationUser()
            {
                DisplayName = User.DisplayName,
                UserName = User.UserName,
                Email = User.Email,
                AddressId = User.AddressId
            };


            IdentityResult Result = await UserManager.CreateAsync(UserToDb, User.Password);

            if(!Result.Succeeded)
            {
                foreach(var E in Result.Errors)
                {
                    Errors.Add(E.Description);
                }
            }

            return Errors;
        }

        public async Task<LoginResponseDTO> Login(LoginDTO User)
        {
            ApplicationUser UserFromDb = await UserManager.FindByEmailAsync(User.Email);

            if(UserFromDb is null)
            {
                return null;
            }

            bool Found = await UserManager.CheckPasswordAsync(UserFromDb, User.Password);

            if(!Found)
            {
                return null;
            }

            // Generate JWT
            var Response = new LoginResponseDTO();

            Response.DisplayName = UserFromDb.DisplayName;

            Response.Token = await new AccountHelper(Config, UserManager).GenerateJwt(UserFromDb);

            Response.ExpireTime = DateTime.Now.AddDays(int.Parse(Config["Jwt:ExpireHours"]));

            return Response;
        }

        public async Task<bool> CheckUniqueEmail(string Email)
        {
            var User = await UserManager.FindByEmailAsync(Email);

            if(User is null)
            {
                return true;
            }

            return false;
        }

        public async Task<UserInfoDTO> GetCurrentUser(string Email)
        {
            var UserFromDb = UserManager.FindByEmailWithAddress(Email);

            if(UserFromDb is null)
            {
                return null;
            }

            UserInfoDTO User = new UserInfoDTO()
            {
                DisplayName = UserFromDb.DisplayName,
                Email = UserFromDb.Email,
                Address = UserFromDb.Address
            };

            return User;
        }
    }
}
