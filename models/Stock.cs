using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace models
{
    public partial class Stock
    {
        [Key]
        public int StockId { get; set; }
        public int Aantal { get; set; }

        // Navigation property's
        public int VestigingId { get; set; }
        public Vestiging Vestiging { get; set; }

        public int ArtikelId { get; set; }
        public Artikel Artikel { get; set; }
    }
}
