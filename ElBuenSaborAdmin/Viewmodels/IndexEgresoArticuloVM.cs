using ElBuenSaborAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSaborAdmin.Viewmodels
{
    public class IndexEgresoArticuloVM
    {
        public List<EgresoArticulo> EgresosArticulos { get; set; }

        public long? ArticuloID { get; set; }

        public long? StockID { get; set; }
    }
}
