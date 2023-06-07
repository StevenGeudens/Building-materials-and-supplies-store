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
using Accessibility;

namespace wpf.ViewModels
{
    public class StockViewModel : BasisViewModel
    {
        private IUnitOfWork _unitOfWork = new UnitOfWork(new KipcornDbContext());

        private Categorie _geselecteerdeCategorie;
        private string _artikelNaam;
        private string _geselecteerdeSortering;

        private DispatcherTimer _zoekTimer;

        public ObservableCollection<Categorie> Categorieen { get; set; }
        public Categorie GeselecteerdeCategorie
        {
            get { return _geselecteerdeCategorie; }
            set { _geselecteerdeCategorie = value; HaalArtikelsOp(); }
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
        public bool ZoekenIsEnabled { get; set; }
        public bool SorterenIsEnabled { get; set; }
        public bool CategorieFilterIsEnabled { get; set; }

        public override string this[string columnName]
        {
            get { return string.Empty; }
        }

        public StockViewModel()
        {
            Categorieen = new(_unitOfWork.CategorieRepo.Ophalen());
            Sortering = new() { "Naam A - Z", "Naam Z - A", "Laagste prijs eerst", "Hoogste prijs eerst" };
            ZoekenIsEnabled = false;
            SorterenIsEnabled = false;
            CategorieFilterIsEnabled = false;
            _zoekTimer = new() // Vertraging voor het zoeken van artikels bij user input (500ms)
            {
                Interval = TimeSpan.FromMilliseconds(500)
            };
            _zoekTimer.Tick += ArtikelsZoeken;
            HaalArtikelsOp();
        }

        public void HaalArtikelsOp()
        {
            if (GeselecteerdeCategorie != null)
            {
                Artikels = new(_unitOfWork.ArtikelRepo.Ophalen(a => a.CategorieId == GeselecteerdeCategorie.CategorieId));
            }
            else
            {
                Artikels = new(_unitOfWork.ArtikelRepo.Ophalen());
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
            }
            else
            {
                Artikels = new(_unitOfWork.ArtikelRepo.Ophalen(a => a.Naam.Contains(ArtikelNaam)));
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
                case "ArtikelBewerken":
                case "ArtikelVerwijderen":
                    if (GeselecteerdArtikel != null) return true;
                    return false;
                case "ArtikelToevoegen":
                    return true;
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
                case "ArtikelBewerken":
                    var AanpassenView = new ArtikelAanpassenView();
                    var AanpassenVm = new ArtikelAanpassenViewModel(AanpassenView, GeselecteerdArtikel.ArtikelId);
                    AanpassenView.DataContext = AanpassenVm;
                    AanpassenView.ShowDialog();
                    _unitOfWork.ArtikelRepo.Reload(GeselecteerdArtikel);
                    if (!string.IsNullOrEmpty(ArtikelNaam))
                    {
                        ArtikelsZoeken(new object(), new EventArgs());
                    }
                    else
                    {
                        HaalArtikelsOp();
                    }
                    break;
                case "ArtikelVerwijderen":
                    MessageBoxResult result = MessageBox.Show("Bent u zeker dat u het volgende artikel wilt verwijderen?" + Environment.NewLine +
                        GeselecteerdArtikel.Naam, "Verwijder", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                    if(result == MessageBoxResult.OK)
                    {
                        _unitOfWork.ArtikelRepo.Verwijderen(GeselecteerdArtikel);
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
                case "ArtikelToevoegen":
                    var ToevoegenView = new ArtikelToevoegenView();
                    var ToevoegenVm = new ArtikelToevoegenViewModel(ToevoegenView);
                    ToevoegenView.DataContext = ToevoegenVm;
                    ToevoegenView.ShowDialog();

                    if (!string.IsNullOrEmpty(ArtikelNaam))
                    {
                        ArtikelsZoeken(new object(), new EventArgs());
                    }
                    else
                    {
                        HaalArtikelsOp();
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
