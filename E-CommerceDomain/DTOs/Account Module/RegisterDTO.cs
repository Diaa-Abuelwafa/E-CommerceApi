using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceDomain.DTOs.Account_Module
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "DispalyName Required")]
        public string DisplayName { get; set; }

        [Required(ErrorMessage = "UserName Required")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmedPassword { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public int AddressId { get; set; }
    }
}
