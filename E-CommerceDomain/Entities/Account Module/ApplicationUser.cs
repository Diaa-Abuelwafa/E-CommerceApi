using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceDomain.Entities.Account_Module
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public int? AddressId { get; set; }
        public Address Address { get; set; }
    }
}
