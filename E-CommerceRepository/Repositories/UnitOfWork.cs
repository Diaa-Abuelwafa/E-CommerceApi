using E_CommerceDomain.Entities;
using E_CommerceDomain.Interfaces;
using E_CommerceRepository.Data.Contexts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceRepository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext Context;

        public Hashtable Repositories { get; set; }

        public UnitOfWork(AppDbContext Context)
        {
            this.Repositories = new Hashtable();
            this.Context = Context;
        }
        public IGenericRepository<TEntity, TKey> Repo<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var Type = typeof(TEntity).Name;

            if(!Repositories.ContainsKey(Type))
            {
                Repositories.Add(Type, new GenericRepository<TEntity, TKey>(Context));
            }

            return (IGenericRepository<TEntity, TKey>)Repositories[Type];
        }

        public int SaveChanges()
        {
            return Context.SaveChanges();
        }
    }
}
