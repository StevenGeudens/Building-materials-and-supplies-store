using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [DataType(DataType.Date)]
        public DateTime OrderDatum { get; set; }
        public int BtwPercentage { get; set; }

        // Navigation property's
        [ForeignKey("KlantId")]
        public int KlantId { get; set; }
        public Klant Klant { get; set; }

        public List<Orderlijn> Orderlijnen { get; set; }
    }
}
