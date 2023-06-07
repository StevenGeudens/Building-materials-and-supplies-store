using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace models
{
    public class Artikel
    {
        [Key]
        public int ArtikelId { get; set; }

        [Required]
        public string Naam { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public decimal Prijs { get; set; }

        [Required]
        public bool EcoCheques { get; set; }

        // Navigation property's
        public List<Stock> StockVestigingen { get; set; }

        public List<Orderlijn> Orderlijnen { get; set; }

		public List<WinkelmandItem> WinkelmandItems { get; set; }

		public int CategorieId { get; set; }
        public Categorie Categorie { get; set; }
    }
}
