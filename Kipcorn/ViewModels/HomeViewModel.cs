using dal;
using dal.Data.UnitOfWork;
using models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace wpf.ViewModels
{
    public class HomeViewModel : BasisViewModel
    {
        private IUnitOfWork _unitOfWork = new UnitOfWork(new KipcornDbContext());

        private Categorie _geselecteerdeCategorie;
        private string _artikelNaam;
        private string _geselecteerdeSortering;
        private Vestiging _geselecteerdeVestiging;

        private DispatcherTimer _zoekTimer;

        public ObservableCollection<Categorie> Categorieen { get; set; }
        public Categorie GeselecteerdeCategorie
        {
            get { return _geselecteerdeCategorie; }
            set { _geselecteerdeCategorie = value; HaalArtikelsOp();}
        }
        public List<Artikel> Artikels { get; set; }
        public string ArtikelNaam 
        {
            get { return _artikelNaam; }
            set 
            {
                _artikelNaam = value;
                if (_zoekTimer.IsEnabled) _zoekTimer.Stop();
                _zoekTimer.Start();
            }
        }
        public ObservableCollection<string> Sortering { get; set; }
        public string GeselecteerdeSortering
        {
            get { return _geselecteerdeSortering; }
            set { _geselecteerdeSortering = value; SorteerArtikels(); }
        }
        public Artikel GeselecteerdArtikel { get; set; }
        public ObservableCollection<Vestiging> Vestigingen { get; set; }
        public Vestiging GeselecteerdeVestiging
        {
            get { return _geselecteerdeVestiging; }
            set 
            {
                _geselecteerdeVestiging = value;
                if (!string.IsNullOrEmpty(ArtikelNaam))
                {
                    ArtikelsZoeken(new object(), new EventArgs());
                }
                else
                {
                    HaalArtikelsOp();
                }
            }
        }
        public bool ZoekenIsEnabled { get; set; }
        public bool SorterenIsEnabled { get; set; }
        public bool CategorieFilterIsEnabled { get; set; }

        public override string this[string columnName]
        {
            get { return string.Empty; }
        }

        public delegate void WinkelmandHandler();
        public event WinkelmandHandler WinkelmandUpdateEvent;

        public HomeViewModel()
        {
            Categorieen = new(_unitOfWork.CategorieRepo.Ophalen());
            Vestigingen = new(_unitOfWork.VestigingRepo.Ophalen());
            Sortering = new() {"Naam A - Z", "Naam Z - A", "Laagste prijs eerst", "Hoogste prijs eerst" };
            ZoekenIsEnabled = false;
            SorterenIsEnabled = false;
            CategorieFilterIsEnabled = false;
            _zoekTimer = new() // Vertraging voor het zoeken van artikels bij user input (500ms)
            {
                Interval = TimeSpan.FromMilliseconds(500)
            };
            _zoekTimer.Tick += ArtikelsZoeken;
        }

        public void HaalArtikelsOp()
        {
            if (GeselecteerdeCategorie != null)
            {
                Artikels = new(_unitOfWork.ArtikelRepo.Ophalen(a => a.CategorieId == GeselecteerdeCategorie.CategorieId));
                List<Stock> stocks = _unitOfWork.StockRepo.Ophalen(s => s.VestigingId == GeselecteerdeVestiging.VestigingId).ToList();
                Artikels.ForEach(a => a.StockVestigingen[0] = stocks.Find(s => s.ArtikelId == a.ArtikelId));
            }
            else
            {
                Artikels = new(_unitOfWork.ArtikelRepo.Ophalen());
                List<Stock> stocks = _unitOfWork.StockRepo.Ophalen(s => s.VestigingId == GeselecteerdeVestiging.VestigingId).ToList();
                Artikels.ForEach(a => a.StockVestigingen[0] = stocks.Find(s => s.ArtikelId == a.ArtikelId));
            }
            ZoekenIsEnabled = true;
            SorterenIsEnabled = true;
            CategorieFilterIsEnabled = true;
            GeselecteerdeSortering = Sortering[0]; // Standaard sortering 'Naam A - Z'
            SorteerArtikels();
        }

        private void ArtikelsZoeken(object sender, EventArgs e) 
        {
            _zoekTimer.Stop();
            if (GeselecteerdeCategorie != null)
            {
                Artikels = new(_unitOfWork.ArtikelRepo.Ophalen(a => a.Naam.Contains(ArtikelNaam) &&
                    a.CategorieId == GeselecteerdeCategorie.CategorieId));
                List<Stock> stocks = _unitOfWork.StockRepo.Ophalen(s => s.VestigingId == GeselecteerdeVestiging.VestigingId).ToList();
                Artikels.ForEach(a => a.StockVestigingen[0] = stocks.Find(s => s.ArtikelId == a.ArtikelId));
            }
            else
            {
                Artikels = new(_unitOfWork.ArtikelRepo.Ophalen(a => a.Naam.Contains(ArtikelNaam)));
                List<Stock> stocks = _unitOfWork.StockRepo.Ophalen(s => s.VestigingId == GeselecteerdeVestiging.VestigingId).ToList();
                Artikels.ForEach(a => a.StockVestigingen[0] = stocks.Find(s => s.ArtikelId == a.ArtikelId));
            }
            if (GeselecteerdeSortering != null) SorteerArtikels();
        }

        public void SorteerArtikels()
        {
            switch (GeselecteerdeSortering)
            {
                case "Naam A - Z":
                    Artikels = new(Artikels.OrderBy(a => a.Naam));
                    break;
                case "Naam Z - A":
                    Artikels = new(Artikels.OrderByDescending(a => a.Naam));
                    break;
                case "Laagste prijs eerst":
                    Artikels = new(Artikels.OrderBy(a => a.Prijs));
                    break;
                case "Hoogste prijs eerst":
                    Artikels = new(Artikels.OrderByDescending(a => a.Prijs));
                    break;
            }
        }

        public override bool CanExecute(object parameter)
        {
            switch (parameter.ToString())
            {
                case "VerwijderCatFilter":
                    if (GeselecteerdeCategorie != null) return true;
                    return false;
                case "ToevoegenAanWinkelwagen":
                    // Enable toevoegen aan winkelwagen knop wanneer er een artikel is geselecteerd en de stock van dit artikel in de geselecteerde vestiging niet 0 is
                    if (GeselecteerdArtikel != null && GeselecteerdArtikel.StockVestigingen.FirstOrDefault(s => s.VestigingId == GeselecteerdeVestiging.VestigingId && s.ArtikelId == GeselecteerdArtikel.ArtikelId).Aantal != 0)
                    {
                        return true;
                    }
                    return false;
                default: return false;
            }
        }

        public override void Execute(object parameter)
        {
            switch (parameter.ToString())
            {
                case "VerwijderCatFilter":
                    GeselecteerdeCategorie = null;
                    break;
                case "ToevoegenAanWinkelwagen":
                    WinkelmandItem? winkelmandItem = _unitOfWork.WinkelmandItemRepo.Ophalen(w => w.ArtikelId == GeselecteerdArtikel.ArtikelId).FirstOrDefault();
                    if(winkelmandItem != null)
                    {
                        winkelmandItem.Aantal++;
                    }
                    else
                    {
                        winkelmandItem = new WinkelmandItem()
                        {
                            Aantal = 1,
                            ArtikelId = GeselecteerdArtikel.ArtikelId
                        };
                    }
                    _unitOfWork.WinkelmandItemRepo.ToevoegenOfAanpassen(winkelmandItem);
                    if (_unitOfWork.Save() == 0)
                    {
                        MessageBox.Show("Kon artikel niet toevoegen aan winkelwagen.", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        WinkelmandUpdateEvent?.Invoke();
                        GeselecteerdArtikel.StockVestigingen[0].Aantal--;
                        _unitOfWork.StockRepo.Aanpassen(GeselecteerdArtikel.StockVestigingen[0]);
                        _unitOfWork.Save();
                        if (!string.IsNullOrEmpty(ArtikelNaam))
                        {
                            ArtikelsZoeken(new object(), new EventArgs());
                        }
                        else
                        {
                            HaalArtikelsOp();
                        }
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
