using E_CommerceDomain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceDomain.Interfaces
{
    public interface IUnitOfWork
    {
        public IGenericRepository<TEntity, TKey> Repo<TEntity, TKey>() where TEntity : BaseEntity<TKey>;
        public int SaveChanges();
    }
}
