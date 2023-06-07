using models.Partials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace models
{
    public partial class Klant : BasisKlasse
    {
		public override string this[string columnName]
		{
			get
			{
				if (columnName == nameof(KlantId) && KlantId <= 0)
				{
					return "KlantId moet een positief getal zijn!";
				}
				if (columnName == nameof(Naam) && string.IsNullOrWhiteSpace(Naam))
				{
					return "Naam is verplicht!";
				}
				if (columnName == nameof(Telefoon) && string.IsNullOrWhiteSpace(Telefoon))
				{
					if (string.IsNullOrWhiteSpace(Telefoon))
					{
						return "Telefoon nummer is verplicht!";
					}
					else if (!int.TryParse(Telefoon, out int _))
					{
						return "Gelieve een geldige Telefoon nummer in te vullen!";
					}
				}
				if (columnName == nameof(Straat) && string.IsNullOrWhiteSpace(Straat))
				{
					return "Straat is verplicht!";
				}
				if (columnName == nameof(HuisNr) && string.IsNullOrWhiteSpace(HuisNr))
				{
					return "Huis nummer is verplicht!";
				}
				if (columnName == nameof(Plaats) && string.IsNullOrWhiteSpace(Plaats))
				{
					return "Plaats is verplicht!";
				}
				if (columnName == nameof(Postcode))
				{
					if (string.IsNullOrWhiteSpace(Postcode))
					{
						return "Plaats is verplicht!";
					}
					else if (!int.TryParse(Postcode, out int _))
					{
						return "Gelieve een geldige postcode in te geven!";
					}
				}
				if (columnName == nameof(Email) && string.IsNullOrWhiteSpace(Email))
				{
					return "Email is verplicht!";
				}
				return string.Empty;
			}
		}

		public override string ToString()
        {
            return this.Naam;
        }
    }
}
