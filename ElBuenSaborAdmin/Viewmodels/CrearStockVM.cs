using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElBuenSaborAdmin.Models;

namespace ElBuenSaborAdmin.Viewmodels
{
    public class CrearStockVM
    {
        public int CantidadCompradorProveedor { get; set; }
        public double PrecioCompra { get; set; }
        public DateTime FechaCompra { get; set; }
        public int CantidadDisponible { get; set; }
        public long ArticuloID { get; set; }

    }
}
