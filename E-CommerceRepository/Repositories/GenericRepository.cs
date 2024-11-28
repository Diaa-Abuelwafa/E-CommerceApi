using E_CommerceDomain.Entities;
using E_CommerceDomain.Interfaces;
using E_CommerceDomain.Interfaces.Specifications;
using E_CommerceRepository.Data.Contexts;
using E_CommerceRepository.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceRepository.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly AppDbContext Context;

        public GenericRepository(AppDbContext Context)
        {
            this.Context = Context;
        }

        public IQueryable<TEntity> GetAll(ISpecifications<TEntity, TKey> Spec)
        {
            var Products = SpecificationsEvaluator<TEntity, TKey>.GetQuery(Context.Set<TEntity>(), Spec);

            return Products;
        }

        public IQueryable<TEntity> GetById(ISpecifications<TEntity, TKey> Spec)
        {
            var Product = SpecificationsEvaluator<TEntity, TKey>.GetQuery(Context.Set<TEntity>(), Spec);

            return Product;
        }

        public bool Add(TEntity Item)
        {
            var Result = Context.Add(Item);

            if(Result is not null)
            {
                return true;
            }

            return false;
        }
        public bool Update(TEntity Item)
        {
            var Result = Context.Update(Item);

            if (Result is not null)
            {
                return true;
            }

            return false;
        }

        public bool Delete(TKey Id)
        {
            var Item = Context.Set<TEntity>().Find(Id);

            if(Item is not null)
            {
                Context.Remove(Item);

                return true;
            }

            return false;
        }

        public int CountItems(ISpecifications<TEntity, TKey> Spec)
        {
            return SpecificationsEvaluator<TEntity, TKey>.GetQuery(Context.Set<TEntity>(), Spec).Count();
        }

        public TEntity GetByIdWithoutSpecifications(TKey Id)
        {
            return Context.Set<TEntity>().Find(Id);
        }
    }
}
