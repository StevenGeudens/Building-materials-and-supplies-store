using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace models
{
    public partial class Stock
    {
        public override string ToString()
        {
            return this.Aantal.ToString();
        }
    }
}
