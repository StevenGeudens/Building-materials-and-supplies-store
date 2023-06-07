using dal.Data.Repositories;
using models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dal.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        #region attributen

        private IRepository<Artikel> _artikelRepo;
        private IRepository<Vestiging> _vestigingRepo;
        private IRepository<Categorie> _categorieRepo;
        private IRepository<Klant> _basisKlantRepo;
        private IRepository<WinkelmandItem> _winkelmandItemRepo;
        private IRepository<Order> _orderRepo;
        private IRepository<Orderlijn> _orderlijnRepo;
        private IRepository<Stock> _stockRepo;

        #endregion attributen

        protected KipcornDbContext Context;
        public UnitOfWork(KipcornDbContext ctx) { Context = ctx; }

        #region repositories

        public IRepository<Artikel> ArtikelRepo
        {
            get
            {
                if (_artikelRepo == null) _artikelRepo = new Repository<Artikel>(Context);
                return _artikelRepo;
            }
        }
        public IRepository<Vestiging> VestigingRepo
        {
            get
            {
                if (_vestigingRepo == null) _vestigingRepo = new Repository<Vestiging>(Context);
                return _vestigingRepo;
            }
        }
        public IRepository<Categorie> CategorieRepo
        {
            get
            {
                if (_categorieRepo == null) _categorieRepo = new Repository<Categorie>(Context);
                return _categorieRepo;
            }
        }
        public IRepository<Klant> KlantRepo
        {
            get
            {
                if (_basisKlantRepo == null) _basisKlantRepo = new Repository<Klant>(Context);
                return _basisKlantRepo;
            }
        }
        public IRepository<WinkelmandItem> WinkelmandItemRepo
        {
            get
            {
                if (_winkelmandItemRepo == null) _winkelmandItemRepo = new Repository<WinkelmandItem>(Context);
                return _winkelmandItemRepo;
            }
        }
        public IRepository<Order> OrderRepo
        {
            get
            {
                if (_orderRepo == null) _orderRepo = new Repository<Order>(Context);
                return _orderRepo;
            }
        }
        public IRepository<Orderlijn> OrderlijnRepo
        {
            get
            {
                if (_orderlijnRepo == null) _orderlijnRepo = new Repository<Orderlijn>(Context);
                return _orderlijnRepo;
            }
        }
        public IRepository<Stock> StockRepo
        {
            get
            {
                if (_stockRepo == null) _stockRepo = new Repository<Stock>(Context);
                return _stockRepo;
            }
        }

        #endregion repositories

        public int Save()
        {
            return Context.SaveChanges();
        }
        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
