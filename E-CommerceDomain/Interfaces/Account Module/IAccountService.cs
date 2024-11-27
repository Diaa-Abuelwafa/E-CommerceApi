using E_CommerceDomain.DTOs.Account_Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceDomain.Interfaces.Account_Module
{
    public interface IAccountService
    {
        public Task<List<string>> Register(RegisterDTO User);

        public Task<LoginResponseDTO> Login(LoginDTO User);
        public Task<bool> CheckUniqueEmail(string Email);
        public Task<UserInfoDTO> GetCurrentUser(string Email);
    }
}
