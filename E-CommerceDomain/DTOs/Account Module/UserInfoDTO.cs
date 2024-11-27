using E_CommerceDomain.Entities.Account_Module;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceDomain.DTOs.Account_Module
{
    public class UserInfoDTO
    {
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; }
    }
}
