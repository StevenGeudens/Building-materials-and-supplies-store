using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace models
{
    public partial class Vestiging
    {
        [Key]
        public int VestigingId { get; set; }

        [Required]
        public string Naam { get; set; }

        [Required]
        public string Telefoon { get; set; }

        [Required]
        public string Straat { get; set; }

        [Required]
        public string HuisNr { get; set; }

        [Required]
        public string Plaats { get; set; }

        [Required]
        public string Postcode { get; set; }

        // Navigation property's
        public List<Stock> StockArtikellen { get; set; }
    }
}
