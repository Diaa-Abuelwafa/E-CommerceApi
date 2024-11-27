using E_CommerceDomain.Entities.Account_Module;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceRepository.Helpers
{
    public static class ExtentionHelper
    {
        public static ApplicationUser FindByEmailWithAddress(this UserManager<ApplicationUser> UserManager, string Email)
        {
            ApplicationUser UserFromDb = UserManager.Users.Include(x => x.Address).FirstOrDefault(x => x.Email == Email);

            if (UserFromDb is not null)
            {
                return UserFromDb;
            }

            return null;
        }
    }
}
