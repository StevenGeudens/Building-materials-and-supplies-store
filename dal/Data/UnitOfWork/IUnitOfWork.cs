using dal.Data.Repositories;
using models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dal.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Artikel> ArtikelRepo { get; }
        IRepository<Vestiging> VestigingRepo { get; }
        IRepository<Categorie> CategorieRepo { get; }
        IRepository<Klant> KlantRepo { get; }
        IRepository<WinkelmandItem> WinkelmandItemRepo { get; }
        IRepository<Order> OrderRepo { get; }
        IRepository<Orderlijn> OrderlijnRepo { get; }
        IRepository<Stock> StockRepo { get; }

        int Save();
    }
}
