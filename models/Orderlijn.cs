using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace models
{
    public class Orderlijn
    {
        [Key]
        public int OrderlijnId { get; set; }

        public int Aantal { get; set; }

        // Navigation property's
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ArtikelId { get; set; }
        public Artikel Artikel { get; set; }
    }
}
