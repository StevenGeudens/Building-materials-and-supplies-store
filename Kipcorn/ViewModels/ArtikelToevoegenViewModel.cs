using dal.Data.UnitOfWork;
using dal;
using models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpf.Views;
using System.Windows;
using System.Security.Permissions;

namespace wpf.ViewModels
{
    public class ArtikelToevoegenViewModel : BasisViewModel
    {
        private IUnitOfWork _unitOfWork = new UnitOfWork(new KipcornDbContext());
        private readonly Window _view;

        public string Naam { get; set; }
        public ObservableCollection<Categorie> Categorieen { get; set; }
        public Categorie GeselecteerdeCategorie { get; set; }
        public string Prijs { get; set; }
        public bool EcoCheques { get; set; }
        public ObservableCollection<Stock> Stocks { get; set; }
        public Stock GeselecteerdeStock { get; set; }
        public string Aantal { get; set; }

        // AantalError word getoond ipv. de default error via de 'this' methode
        // omdat een stock niet verplicht is voor een artikel en dit het aanmaken van het artikel
        // via de .IsGeldig() methode zou tegenhouden
        public string AantalError { get; set; } 
        public ObservableCollection<Vestiging> Vestigingen { get; set; }
        public Vestiging GeselecteerdeVestiging { get; set; }
        public override string this[string columnName]
        {
            get 
            {
                if (columnName == nameof(Naam) && string.IsNullOrWhiteSpace(Naam))
                {
                    return "Naam is een verplicht veld!";
                }
                if (columnName == nameof(GeselecteerdeCategorie) && !(GeselecteerdeCategorie is Categorie))
                {
                    return "Gelieve een categorie te selecteren!";
                }
                if (columnName == nameof(Prijs))
                {
                    if(decimal.TryParse(Prijs, out decimal prijs))
                    {
                        if(prijs <= 0)
                        {
                            return "Wij geven geen cadeau's!";
                        }
                    }
                    else
                    {
                        return "Gelieve een geldige prijs in te geven!";
                    }
                }
                return string.Empty;
            }
        }

        public ArtikelToevoegenViewModel(Window view)
        {
            _view = view;
            Stocks = new();
            Categorieen = new(_unitOfWork.CategorieRepo.Ophalen());
            Vestigingen = new (_unitOfWork.VestigingRepo.Ophalen());
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
                            if(aantal >= 0)
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
                    if(this.IsGeldig()) return true;
                    return false;
                default: return false;
            }
        }

        public override void Execute(object parameter)
        {
            switch (parameter.ToString())
            {
                case "StockToevoegen":
                    Stocks.Add(new Stock() 
                    {
                        Aantal = int.Parse(Aantal),
                        ArtikelId = 0, // ArtikelId kan pas worden toegevoegd na het opslaan van dit nieuw artikel in de DB
                        VestigingId = GeselecteerdeVestiging.VestigingId,
                        Vestiging = GeselecteerdeVestiging
                    });
                    Vestigingen.Remove(GeselecteerdeVestiging);
                    break;
                case "StockVerwijderen":
                    Vestigingen.Add(_unitOfWork.VestigingRepo.Ophalen(v => v.VestigingId == GeselecteerdeStock.VestigingId).FirstOrDefault());
                    Stocks.Remove(GeselecteerdeStock);
                    break;
                case "ArtikelOpslaan":
                    Artikel artikelCheck = _unitOfWork.ArtikelRepo.Ophalen(a => a.Naam == Naam).FirstOrDefault();
                    if (artikelCheck == null) // Check of dat het artikel met deze naam al bestaad
                    {
                        Artikel newArtikel = new Artikel()
                        {
                            Naam = Naam,
                            Prijs = decimal.Parse(Prijs),
                            EcoCheques = EcoCheques,
                            CategorieId = GeselecteerdeCategorie.CategorieId
                        };
                        _unitOfWork.ArtikelRepo.Toevoegen(newArtikel);
                        _unitOfWork.Save();
                        // Atikel stocks toevoegen
                        foreach (Stock stock in Stocks)
                        {
                            stock.ArtikelId = newArtikel.ArtikelId;
                        }
                        // Alle vestigingen zonder gedefinieerde stock krijgen een stock van aantal 0 toegewezen
                        foreach (Vestiging vestiging in Vestigingen)
                        {
                            Stocks.Add(new Stock()
                            {
                                Aantal = 0,
                                ArtikelId = newArtikel.ArtikelId,
                                VestigingId = vestiging.VestigingId
                            });
                        }
                        _unitOfWork.StockRepo.ToevoegenRange(Stocks);
                        _unitOfWork.Save();
                        _view.Close();
                    }
                    else
                    {
                        MessageBox.Show($"Een artikel met de naam: '{Naam}' bestaat al!", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;
            }
        }

        public void Dispose()
        {
            _unitOfWork?.Dispose();
        }
    }
}
