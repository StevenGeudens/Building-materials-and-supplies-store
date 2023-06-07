using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dal.Data.Repositories
{
    public class Repository<T> : IRepository<T>
        where T : class, new()
    {
        protected KipcornDbContext Context { get; }
        public Repository(KipcornDbContext context)
        {
            this.Context = context;
        }

        #region Ophalen

        public IEnumerable<T> Ophalen()
        {
            return Context.Set<T>().ToList();
        }
        public IEnumerable<T> Ophalen(Expression<Func<T, bool>> voorwaarden,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = Context.Set<T>();
            if(includes != null)
            {
                foreach(var item in includes)
                {
                    query = query.Include(item);
                }
            }
            if(voorwaarden != null)
            {
                query = query.Where(voorwaarden);
            }
            return query.ToList();
        }
        public IEnumerable<T> Ophalen(Expression<Func<T, bool>> voorwaarden)
        {
            return Ophalen(voorwaarden, null).ToList();
        }
        public IEnumerable<T> Ophalen(params Expression<Func<T, object>>[] includes)
        {
            return Ophalen(null, includes).ToList();
        }

        #endregion Ophalen

        public void Toevoegen(T entity)
        {
            Context.Set<T>().Add(entity);
        }
        public void Aanpassen(T entity)
        {
            Context.Entry<T>(entity).State = EntityState.Modified;
        }
        public void Verwijderen(T entity)
        {
            Context.Entry(entity).State = EntityState.Deleted;
        }
        public void ToevoegenOfAanpassen(T entity)
        {
            // Wanneer de PK = 0, zal er worden toegevoegd
            // Wanneer de PK > 0, zal er worden geupdatet
            Context.Set<T>().Update(entity);
        }
        public void ToevoegenRange(IEnumerable<T> entities)
        {
            Context.Set<T>().AddRange(entities);
        }
        public void ToevoegenOfAanpassenRange(IEnumerable<T> entities)
        {
            Context.Set<T>().UpdateRange(entities);
        }

        public void Reload(T entity)
        {
            Context.Entry(entity).Reload();
        }
    }
}
