using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace models
{
    public class WinkelmandItem
    {
        [Key]
        public int WinkelmandItemId { get; set; }

        [Required]
        public int Aantal { get; set; }

        // nav props
        public int ArtikelId { get; set; }
        public Artikel Artikel { get; set; }
    }
}
