using dal.Data.UnitOfWork;
using dal;
using Microsoft.Win32;
using models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace wpf.ViewModels
{
	public class KlantToevoegenViewModel : BasisViewModel
	{
		private readonly Window _view;
		private IUnitOfWork _unitOfWork = new UnitOfWork(new KipcornDbContext());

		public string Naam { get; set; }
		public string Telefoon { get; set; }
		public string Straat { get; set; }
		public string Nummer { get; set; }
		public string Postcode { get; set; }
		public string Plaats { get; set; }
		public string Email { get; set; }
		public string? BtwNummer { get; set; }

		public override string this[string columnName] 
		{
			get
			{
				if (columnName == nameof(Naam) && string.IsNullOrWhiteSpace(Naam))
				{
					return "Naam is een verplicht veld!";
				}
				if (columnName == nameof(Telefoon) && string.IsNullOrWhiteSpace(Telefoon))
				{
					return "Telefoon is een verplicht veld!";
				}
				if (columnName == nameof(Straat) && string.IsNullOrWhiteSpace(Straat))
				{
					return "Straat is een verplicht veld!";
				}
				if (columnName == nameof(Nummer) && string.IsNullOrWhiteSpace(Nummer))
				{
					return "Nummer is een verplicht veld!";
				}
				if (columnName == nameof(Postcode))
				{
					if (string.IsNullOrWhiteSpace(Postcode))
					{
						return "Postcode is een verplicht veld!";
					}
					else if (!int.TryParse(Postcode, out int _))
					{
						return "Gelieve een geldige postcode in te vullen!";
					}

				}
				if (columnName == nameof(Plaats) && string.IsNullOrWhiteSpace(Plaats))
				{
					return "Plaats is een verplicht veld!";
				}
				if (columnName == nameof(Email) && string.IsNullOrWhiteSpace(Email))
				{
					return "Email is een verplicht veld!";
				}
				return string.Empty;
			}
		}

		public KlantToevoegenViewModel(Window view)
		{
			_view = view;
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
					Klant nieuweKlant = new Klant()
					{
						Naam = Naam,
						Telefoon = Telefoon,
						Straat = Straat,
						HuisNr = Nummer,
						Plaats = Plaats,
						Postcode = Postcode,
						Email = Email,
						BtwNummer = BtwNummer
					};
					if (nieuweKlant.IsGeldig())
					{
						_unitOfWork.KlantRepo.Toevoegen(nieuweKlant);
						if(_unitOfWork.Save() == 0)
						{
							MessageBox.Show("Klant is niet toegevoegd!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
						}
					}
					_view.Close();
					break;
			}
		}
	}
}
