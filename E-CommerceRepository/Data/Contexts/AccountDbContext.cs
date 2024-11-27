using E_CommerceDomain.Entities.Account_Module;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceRepository.Data.Contexts
{
    public class AccountDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Address> Addresses { get; set; }

        public AccountDbContext(DbContextOptions<AccountDbContext> Options) : base(Options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
