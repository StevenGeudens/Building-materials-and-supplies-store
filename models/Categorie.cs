using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace models
{
    public partial class Categorie
    {
        [Key]
        public int CategorieId { get; set; }

        [Required]
        public string Naam { get; set; }

        // Navigation property's
        public List<Artikel> Artikelen { get; set; }
    }
}
