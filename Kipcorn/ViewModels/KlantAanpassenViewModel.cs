using dal.Data.UnitOfWork;
using dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using models;
using System.IO;

namespace wpf.ViewModels
{
    public class KlantAanpassenViewModel : BasisViewModel
    {
		private IUnitOfWork _unitOfWork = new UnitOfWork(new KipcornDbContext());
		private readonly Window _view;

		public Klant Klant { get; set; }

		public override string this[string columnName]
		{
			get
			{
				if (columnName == nameof(Klant.Naam) && string.IsNullOrWhiteSpace(Klant.Naam))
				{
					return "Naam is een verplicht veld!";
				}
				if (columnName == nameof(Klant.Telefoon) && string.IsNullOrWhiteSpace(Klant.Telefoon))
				{
					if (string.IsNullOrWhiteSpace(Klant.Telefoon))
					{
						return "Telefoon is een verplicht veld!";
					}
					else if (!int.TryParse(Klant.Telefoon, out int _))
					{
						return "Gelieve een geldige Telefoon nummer in te vullen!";
					}
				}
				if (columnName == nameof(Klant.Straat) && string.IsNullOrWhiteSpace(Klant.Straat))
				{
					return "Straat is een verplicht veld!";
				}
				if (columnName == nameof(Klant.HuisNr) && string.IsNullOrWhiteSpace(Klant.HuisNr))
				{
					return "Nummer is een verplicht veld!";
				}
				if (columnName == nameof(Klant.Postcode))
				{
					if (string.IsNullOrWhiteSpace(Klant.Postcode))
					{
						return "Postcode is een verplicht veld!";
					}
					else if (!int.TryParse(Klant.Postcode, out int _))
					{
						return "Gelieve een geldige postcode in te vullen!";
					}

				}
				if (columnName == nameof(Klant.Plaats) && string.IsNullOrWhiteSpace(Klant.Plaats))
				{
					return "Plaats is een verplicht veld!";
				}
				if (columnName == nameof(Klant.Email) && string.IsNullOrWhiteSpace(Klant.Email))
				{
					return "Email is een verplicht veld!";
				}
				return string.Empty;
			}
		}

		public KlantAanpassenViewModel(Window view, int id)
        {
			_view = view;
			Klant = _unitOfWork.KlantRepo.Ophalen(k => k.KlantId == id).FirstOrDefault();
			if (Klant == null)
			{
				MessageBox.Show("Klant niet gevonden!", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
				_view.Close();
			}
		}

		public override bool CanExecute(object parameter)
		{
			switch (parameter.ToString())
			{
				case "KlantOpslaan":
					if (this.IsGeldig()) return true;
					return false;
				default: return false;
			}
		}

		public override void Execute(object parameter)
		{
			switch (parameter.ToString())
			{
				case "KlantOpslaan":
					if (Klant.IsGeldig())
					{
						_unitOfWork.KlantRepo.ToevoegenOfAanpassen(Klant);
						if (_unitOfWork.Save() == 0)
						{
							MessageBox.Show("Klant is niet aangepast!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
						}
					}
					_view.Close();
					break;
			}
		}
	}
}
