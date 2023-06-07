using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dal.Data.Repositories
{
    public interface IRepository<T>
        where T : class, new()
    {
        #region Ophalen

        IEnumerable<T> Ophalen();
        IEnumerable<T> Ophalen(Expression<Func<T, bool>> voorwaarden,
            params Expression<Func<T, object>>[] includes);
        IEnumerable<T> Ophalen(Expression<Func<T, bool>> voorwaarden);
        IEnumerable<T> Ophalen(params Expression<Func<T, object>>[] includes);

        #endregion Ophalen

        void Toevoegen(T entity);

        void Aanpassen(T entity);

        void Verwijderen(T entity);

        void ToevoegenOfAanpassen(T entity);

        void ToevoegenRange(IEnumerable<T> entities);
        void ToevoegenOfAanpassenRange(IEnumerable<T> entities);

        void Reload(T entity);
    }
}
