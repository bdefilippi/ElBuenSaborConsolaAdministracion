using ElBuenSaborAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSaborAdmin.Viewmodels
{
    public class IndexPrecioVentaArticuloVM
    {
        public List<PrecioVentaArticulo> PrecioVentaArticulos { get; set; }

        public long? ArticuloID { get; set; }
    }
}
