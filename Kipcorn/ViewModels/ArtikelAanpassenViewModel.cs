using dal.Data.UnitOfWork;
using dal;
using models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace wpf.ViewModels
{
    public class ArtikelAanpassenViewModel : BasisViewModel
    {
        private IUnitOfWork _unitOfWork = new UnitOfWork(new KipcornDbContext());
        private readonly Window _view;

        public Artikel Artikel { get; set; }
        public ObservableCollection<Categorie> Categorieen { get; set; }

        public ObservableCollection<Stock> Stocks { get; set; }
        public Stock GeselecteerdeStock { get; set; }
        public string Aantal { get; set; }
        // AantalError word getoond ipv. de default error via de 'this' methode
        // omdat een stock niet verplicht is voor een artikel en dit het aanmaken van het artikel
        // via de .IsGeldig() methode zou tegenhouden
        public string AantalError { get; set; }
        public List<Vestiging> Vestigingen { get; set; }
        public Vestiging GeselecteerdeVestiging { get; set; }
        public override string this[string columnName]
        {
            get
            {
                if (columnName == nameof(Artikel.Naam) && Artikel.Naam == null)
                {
                    return "Naam is een verplicht veld!";
                }

                return string.Empty;
            }
        }

        public ArtikelAanpassenViewModel(Window view, int id)
        {
            _view = view;
            Categorieen = new(_unitOfWork.CategorieRepo.Ophalen());
            Artikel = _unitOfWork.ArtikelRepo.Ophalen(a => a.ArtikelId == id, a => a.Categorie, a => a.StockVestigingen).FirstOrDefault();

            if (Artikel != null) 
            {
                Stocks = new(Artikel.StockVestigingen);
                Vestigingen = new();

                // Wanneer er nog een vestiging zou zijn zonder stock wordt deze getoond in de combobox
                List<int> stockVestigingIds = new();
                Artikel.StockVestigingen.ForEach(s => stockVestigingIds.Add(s.VestigingId));
                foreach (Vestiging vestiging in _unitOfWork.VestigingRepo.Ophalen())
                {
                    if (!stockVestigingIds.Contains(vestiging.VestigingId)) Vestigingen.Add(vestiging);
                }
            }
            else
            {
                MessageBox.Show("Artikel niet gevonden!", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                _view.Close();
            }
        }

        public override bool CanExecute(object parameter)
        {
            switch (parameter.ToString())
            {
                case "StockToevoegen":
                    if (!string.IsNullOrEmpty(Aantal) && GeselecteerdeVestiging != null)
                    {
                        if (int.TryParse(Aantal, out int aantal))
                        {
                            if (aantal >= 0)
                            {
                                AantalError = string.Empty;
                                return true;
                            }
                        }
                        AantalError = "Gelieve een geldig" + Environment.NewLine + "aantal in te geven!";

                    }
                    return false;
                case "StockVerwijderen":
                    if (GeselecteerdeStock != null) return true;
                    return false;
                case "ArtikelOpslaan":
                    if (this.IsGeldig()) return true;
                    return false;
                default: return false;
            }
        }

        public override void Execute(object parameter)
        {
            switch (parameter.ToString())
            {
                case "StockToevoegen":
                    Stock newStock = new Stock()
                    {
                        Aantal = int.Parse(Aantal),
                        ArtikelId = Artikel.ArtikelId,
                        VestigingId = GeselecteerdeVestiging.VestigingId,
                        Vestiging = GeselecteerdeVestiging
                    };
                    Stocks.Add(newStock);
                    _unitOfWork.StockRepo.ToevoegenOfAanpassen(newStock);
                    Vestigingen.Remove(GeselecteerdeVestiging);
                    ResetStockToevoegen();
                    break;
                case "StockVerwijderen":
                    Vestigingen.Add(_unitOfWork.VestigingRepo.Ophalen(v => v.VestigingId == GeselecteerdeStock.VestigingId).FirstOrDefault());
                    _unitOfWork.StockRepo.Verwijderen(GeselecteerdeStock);
                    Stocks.Remove(GeselecteerdeStock);
                    break;
                case "ArtikelOpslaan":
                    _unitOfWork.ArtikelRepo.Aanpassen(Artikel);
                    // Alle vestigingen zonder gedefinieerde stock krijgen een stock van aantal 0 toegewezen
                    foreach (Vestiging vestiging in Vestigingen)
                    {
                        Stocks.Add(new Stock()
                        {
                            Aantal = 0,
                            ArtikelId = Artikel.ArtikelId,
                            VestigingId = vestiging.VestigingId
                        });
                    }
                    _unitOfWork.Save();
                    _view.Close();
                    break;
            }
        }

        public void ResetStockToevoegen()
        {
            Aantal = string.Empty;
            GeselecteerdeVestiging = null;
        }

        public void Dispose()
        {
            _unitOfWork?.Dispose();
        }
    }
}
