using E_CommerceDomain.Entities;
using E_CommerceDomain.Interfaces.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceRepository.Helpers
{
    public static class SpecificationsEvaluator<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> Base, ISpecifications<TEntity, TKey> Spec)
        {
            var Query = Base;

            if(Spec.Criteria is not null)
            {
                Query = Query.Where(Spec.Criteria);
            }

            foreach(var I in Spec.Includes)
            {
                Query = Query.Include(I);
            }

            if (Spec.OrderBy is not null)
            {
                Query = Query.OrderBy(Spec.OrderBy);
            }
            else if (Spec.OrderByDesc is not null)
            {
                Query = Query.OrderByDescending(Spec.OrderByDesc);
            }

            if(Spec.Skip != 0)
            {
                Query = Query.Skip(Spec.Skip).Take(Spec.Take);
            }

            return Query;
        }
    }
}
