using dal.Data.UnitOfWork;
using dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using models;
using System.Windows;
using wpf.Views;
using System.Windows.Threading;

namespace wpf.ViewModels
{
    public class KlantenViewModel : BasisViewModel
	{
		private IUnitOfWork _unitOfWork = new UnitOfWork(new KipcornDbContext());
		private string _klantNaamOfEmail;
		private string _geselecteerdeSortering;
		private string _geselecteerdeFilter;
		private DispatcherTimer _zoekTimer;

		public ObservableCollection<Klant> Klanten { get; set; }
		public Klant GeselecteerdeKlant { get; set; }
		public ObservableCollection<string> Sortering { get; set; }
		public string GeselecteerdeSortering
		{
			get { return _geselecteerdeSortering; }
			set { _geselecteerdeSortering = value; SorteerKlanten(); }
		}
		public ObservableCollection<string> Filters { get; set; }
		public string GeselecteerdeFilter
		{
			get { return _geselecteerdeFilter; }
			set { _geselecteerdeFilter = value; FilterKlanten(); }
		}

		public string KlantNaamOfEmail
		{
			get { return _klantNaamOfEmail; }
			set
			{
				_klantNaamOfEmail = value;
				if (_zoekTimer.IsEnabled) _zoekTimer.Stop();
				_zoekTimer.Start();
			}
		}

		public override string this[string columnName]
		{
			get { return string.Empty; }
		}

		public KlantenViewModel()
		{
			Klanten = new(_unitOfWork.KlantRepo.Ophalen());
			Sortering = new() { "Naam A - Z", "Naam Z - A" };
			Filters = new() { "Alle klanten", "Proffesionele klanten", "Niet proffesionele klanten" };
			_zoekTimer = new() // Vertraging voor het zoeken van artikels bij user input (500ms)
			{
				Interval = TimeSpan.FromMilliseconds(500)
			};
			_zoekTimer.Tick += KlantenZoeken;
			GeselecteerdeSortering = Sortering[0]; // Standaard sortering 'Naam A - Z'
			GeselecteerdeFilter = Filters[0]; // Standaard filter 'Alle klanten'
			SorteerKlanten();
		}

		private void KlantenZoeken(object sender, EventArgs e)
		{
			_zoekTimer.Stop();
			Klanten = new(_unitOfWork.KlantRepo.Ophalen(k => k.Naam.Contains(KlantNaamOfEmail) || k.Email.Contains(KlantNaamOfEmail)));
		}
		public void SorteerKlanten()
		{
			switch (GeselecteerdeSortering)
			{
				case "Naam A - Z":
					Klanten = new(Klanten.OrderBy(a => a.Naam));
					break;
				case "Naam Z - A":
					Klanten = new(Klanten.OrderByDescending(a => a.Naam));
					break;
			}
		}
		public void FilterKlanten()
		{
			if (!string.IsNullOrEmpty(KlantNaamOfEmail)) KlantenZoeken(new object(), new EventArgs());
			else
			{
				Klanten = new(_unitOfWork.KlantRepo.Ophalen());
			}

			switch (GeselecteerdeFilter)
			{
				case "Proffesionele klanten":
					Klanten = new(Klanten.Where(k => !string.IsNullOrEmpty(k.BtwNummer)));
					break;
				case "Niet proffesionele klanten":
					Klanten = new(Klanten.Where(k => string.IsNullOrEmpty(k.BtwNummer)));
					break;
				default: break;
			}

			if (!string.IsNullOrEmpty(GeselecteerdeSortering)) SorteerKlanten();
		}

		public override bool CanExecute(object parameter)
		{
			switch (parameter.ToString())
			{
				case "KlantBewerken":
				case "KlantVerwijderen":
					if (GeselecteerdeKlant != null) return true;
					return false;
				case "KlantToevoegen":
					return true;
				default: return false;
			}
		}
		public override void Execute(object parameter)
		{
			switch (parameter.ToString())
			{
				case "KlantBewerken":
					var AanpassenView = new KlantAanpassenView();
					var AanpassenVm = new KlantAanpassenViewModel(AanpassenView, GeselecteerdeKlant.KlantId);
					AanpassenView.DataContext = AanpassenVm;
					AanpassenView.ShowDialog();
					_unitOfWork.KlantRepo.Reload(GeselecteerdeKlant);
					FilterKlanten();
					break;
				case "KlantVerwijderen":
					MessageBoxResult result = MessageBox.Show("Bent u zeker dat u volgende klant wilt verwijderen?" + Environment.NewLine +
					GeselecteerdeKlant.Naam + Environment.NewLine +
					GeselecteerdeKlant.Email, "Verwijder", MessageBoxButton.OKCancel, MessageBoxImage.Question);
					if (result == MessageBoxResult.OK)
					{
						_unitOfWork.KlantRepo.Verwijderen(GeselecteerdeKlant);
						_unitOfWork.Save();
						FilterKlanten();
					}
					break;
				case "KlantToevoegen":
					var ToevoegenView = new KlantToevoegenView();
					var ToevoegenVm = new KlantToevoegenViewModel(ToevoegenView);
					ToevoegenView.DataContext = ToevoegenVm;
					ToevoegenView.ShowDialog();
					FilterKlanten();
					break;
			}
		}

		public void Dispose()
		{
			_unitOfWork?.Dispose();
		}
	}
}
