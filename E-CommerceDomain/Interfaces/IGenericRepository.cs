using E_CommerceDomain.Entities;
using E_CommerceDomain.Interfaces.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceDomain.Interfaces
{
    public interface IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public IQueryable<TEntity> GetAll(ISpecifications<TEntity, TKey> Spec);
        public IQueryable<TEntity> GetById(ISpecifications<TEntity, TKey> Spec);
        public bool Add(TEntity Item);
        public bool Update(TEntity Item);
        public bool Delete(TKey Id);
        public int CountItems(ISpecifications<TEntity, TKey> Spec);
    }
}
