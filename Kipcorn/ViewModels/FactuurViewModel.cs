using dal.Data.UnitOfWork;
using dal;
using models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf.ViewModels
{
    public class FactuurViewModel
    {
        private IUnitOfWork _unitOfWork = new UnitOfWork(new KipcornDbContext());

        public Klant Klant { get; set; }
        public int Factuurnummer { get; set; }
        public DateTime Factuurdatum { get; set; }
        public DateTime Vervaldatum { get; set; }
        public ObservableCollection<Orderlijn> Orderlijnen { get; set; }
        public decimal TotaalPrijsZonderBtw { get; set; }
        public int BtwPercentage { get; set; }
        public decimal BtwBedrag { get; set; }
        public decimal TotaalPrijsMetBtw { get; set; }


        public FactuurViewModel(int orderId)
        {
            Factuurnummer = orderId;
            Order order = _unitOfWork.OrderRepo.Ophalen(o => o.OrderId == orderId, o => o.Orderlijnen).FirstOrDefault();
            order.Orderlijnen.ForEach(ol => ol.Artikel = _unitOfWork.ArtikelRepo.Ophalen(a => a.ArtikelId == ol.ArtikelId).FirstOrDefault());
            Orderlijnen = new(order.Orderlijnen);
            Factuurdatum = order.OrderDatum;
            Vervaldatum = Factuurdatum.AddMonths(1);
            Klant = _unitOfWork.KlantRepo.Ophalen(k => k.KlantId == order.KlantId).FirstOrDefault();
            BtwPercentage = order.BtwPercentage;

            decimal totaalPrijsZonderBtw = 0;
            foreach (Orderlijn orderlijn in Orderlijnen)
            {
                totaalPrijsZonderBtw += (orderlijn.Aantal * orderlijn.Artikel.Prijs);
            }
            TotaalPrijsZonderBtw = totaalPrijsZonderBtw;

            if (BtwPercentage == 0)
            {
                BtwBedrag = 0;
                TotaalPrijsMetBtw = TotaalPrijsZonderBtw;
            }
            else
            {
                BtwBedrag = TotaalPrijsZonderBtw / 100 * BtwPercentage;
                TotaalPrijsMetBtw = TotaalPrijsZonderBtw + BtwBedrag;
            }
        }
    }
}
