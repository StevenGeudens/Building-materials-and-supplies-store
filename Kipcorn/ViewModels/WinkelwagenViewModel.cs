using dal.Data.UnitOfWork;
using dal;
using models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using wpf.Views;

namespace wpf.ViewModels
{
    public class WinkelwagenViewModel : BasisViewModel
    {
        private IUnitOfWork _unitOfWork = new UnitOfWork(new KipcornDbContext());
        private WinkelmandItem _geselecteerdWinkelmandItem;
        private int _geselecteerdWinkelmandItemAantal;
        private int? _geselecteerdBtwPercentage;
        private Klant _geselecteerdeKlant;

        private bool eersteKeer = false;
        public ObservableCollection<WinkelmandItem> WinkelmandItems { get; set; }
        public WinkelmandItem GeselecteerdWinkelmandItem 
        {
            get { return _geselecteerdWinkelmandItem; }
            set 
            {
                _geselecteerdWinkelmandItem = value;
                GeselecteerdWinkelmandItemAantal = 0;
                if (_geselecteerdWinkelmandItem != null)
                {
                    eersteKeer = true;
                    GeselecteerdWinkelmandItemAantal = GeselecteerdWinkelmandItem.Aantal;
                }
            }
        }
        public int GeselecteerdWinkelmandItemAantal 
        {
            get { return _geselecteerdWinkelmandItemAantal; }
            set 
            {
                _geselecteerdWinkelmandItemAantal = value;
                if(_geselecteerdWinkelmandItemAantal != 0 && !eersteKeer)
                {
                    UpdateWinkelmandItemAantal();
                }
                eersteKeer = false;
            }
        }
        public ObservableCollection<Klant> Klanten { get; set; }
        public Klant GeselecteerdeKlant 
        {
            get { return _geselecteerdeKlant; }
            set
            {
                _geselecteerdeKlant = value;
                BtwPercentagesInstellen();
            }
        }
        public ObservableCollection<int> BtwPercentages { get; set; }
        public int? GeselecteerdBtwPercentage 
        {
            get { return _geselecteerdBtwPercentage; }
            set 
            {
                _geselecteerdBtwPercentage = value;
                TotaalPrijsMetBtwBerekenen();
            }
        }
        public bool BtwPercentagesIsEnabled { get; set; }
        public decimal TotaalPrijsZonderBtw { get; set; }
        public decimal BtwBedrag { get; set; }
        public decimal TotaalPrijsMetBtw { get; set; }

        public delegate void WinkelmandHandler();
        public event WinkelmandHandler WinkelmandUpdateEvent;
        public override string this[string columnName]
        {
            get {
                if (columnName == nameof(GeselecteerdeKlant) && GeselecteerdeKlant == null)
                {
                    return "Gelieve een klant te selecteren!";
                }
                if (columnName == nameof(GeselecteerdBtwPercentage) && GeselecteerdBtwPercentage == null)
                {
                    return "Gelieve een btw percentage te selecteren!";
                }
                return string.Empty;
            }
        }

        public WinkelwagenViewModel()
        {
            KlantenOphalen();
            WinkelmandItemsOphalen();
            BtwPercentages = new() { 0, 6, 21 };
            BtwPercentagesIsEnabled = false;
        }

        public override bool CanExecute(object parameter)
        {
            switch (parameter.ToString())
            {
                case "NieweKlantToevoegen":
                    return true;
                case "ArtikelVerwijderen":
                case "ArtikelAantalMin":
                case "ArtikelAantalPlus":
                    if (GeselecteerdWinkelmandItem != null) return true;
                    return false;
                case "FactuurAfdrukken":
                    if (WinkelmandItems.Count() != 0 && this.IsGeldig())
                        return true;
                    return false;
                default: return false;
            }
        }

        public override void Execute(object parameter)
        {
            switch (parameter.ToString())
            {
                case "NieweKlantToevoegen":
                    var ToevoegenView = new KlantToevoegenView();
                    var ToevoegenVm = new KlantToevoegenViewModel(ToevoegenView);
                    ToevoegenView.DataContext = ToevoegenVm;
                    ToevoegenView.ShowDialog();
                    KlantenOphalen();
                    break;
                case "ArtikelVerwijderen":
                    _unitOfWork.WinkelmandItemRepo.Verwijderen(GeselecteerdWinkelmandItem);
                    _unitOfWork.Save();
                    RefreshWinkelwagen();
                    break;
                case "ArtikelAantalMin":
                    if ((GeselecteerdWinkelmandItemAantal - 1) <= 0)
                    {
                        _unitOfWork.WinkelmandItemRepo.Verwijderen(GeselecteerdWinkelmandItem);
                        _unitOfWork.Save();
                    }
                    else
                    {
                        GeselecteerdWinkelmandItemAantal--;
                    }
                    RefreshWinkelwagen();
                    break;
                case "ArtikelAantalPlus":
                    GeselecteerdWinkelmandItemAantal++;
                    RefreshWinkelwagen();
                    break;
                case "FactuurAfdrukken":
                    Order newOrder = new Order()
                    {
                        KlantId = GeselecteerdeKlant.KlantId,
                        OrderDatum = DateTime.Now,
                        BtwPercentage = (int)GeselecteerdBtwPercentage
                    };
                    _unitOfWork.OrderRepo.Toevoegen(newOrder);
                    _unitOfWork.Save();
                    int orderId = _unitOfWork.OrderRepo.Ophalen(o => o.OrderId == newOrder.OrderId).FirstOrDefault().OrderId;
                    foreach(WinkelmandItem winkelmandItem in WinkelmandItems)
                    {
                        _unitOfWork.OrderlijnRepo.Toevoegen(new Orderlijn() 
                        {
                            OrderId = orderId,
                            ArtikelId = winkelmandItem.ArtikelId,
                            Aantal = winkelmandItem.Aantal
                        });
                    }
                    _unitOfWork.Save();
                    var vm = new FactuurViewModel(orderId);
                    var view = new FactuurView();
                    view.DataContext = vm;
                    view.Show();
                    break;
            }
        }

        private void WinkelmandItemsOphalen()
        {
            WinkelmandItems = new(_unitOfWork.WinkelmandItemRepo.Ophalen(w => w.Artikel));
            TotaalPrijsZonderBtwBerekenen();
        }
        private void KlantenOphalen()
        {
            Klanten = new(_unitOfWork.KlantRepo.Ophalen());
        }

        public void TotaalPrijsZonderBtwBerekenen()
        {
            decimal totaalPrijsZonderBtw = 0;
            foreach(WinkelmandItem winkelmandItem in WinkelmandItems)
            {
                totaalPrijsZonderBtw += (winkelmandItem.Aantal * winkelmandItem.Artikel.Prijs);
            }
            TotaalPrijsZonderBtw = totaalPrijsZonderBtw;
        }
        public void TotaalPrijsMetBtwBerekenen()
        {
            if (GeselecteerdBtwPercentage == 0)
            {
                BtwBedrag = 0;
                TotaalPrijsMetBtw = TotaalPrijsZonderBtw;
            }
            else
            {
                BtwBedrag = TotaalPrijsZonderBtw / 100 * (int)GeselecteerdBtwPercentage;
                TotaalPrijsMetBtw = TotaalPrijsZonderBtw + BtwBedrag;
            }
        }

        private void UpdateWinkelmandItemAantal()
        {
            GeselecteerdWinkelmandItem.Aantal = GeselecteerdWinkelmandItemAantal;
            _unitOfWork.WinkelmandItemRepo.Aanpassen(GeselecteerdWinkelmandItem);
            _unitOfWork.Save();
        }

        private void RefreshWinkelwagen()
        {
            WinkelmandUpdateEvent?.Invoke();
            WinkelmandItemsOphalen();
        }

        public void BtwPercentagesInstellen()
        {
            if (GeselecteerdeKlant != null)
            {
                if (string.IsNullOrEmpty(GeselecteerdeKlant.BtwNummer))
                {
                    GeselecteerdBtwPercentage = 21;
                    BtwPercentagesIsEnabled = false;
                }
                else
                {
                    BtwPercentagesIsEnabled = true;
                }
            }
            else
            {
                BtwPercentagesIsEnabled = false;
            }
        }
    }
}
