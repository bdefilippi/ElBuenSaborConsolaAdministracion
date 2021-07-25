using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSaborAdmin.Models
{
    public class Stock
    {
        public long Id { get; set; }
        public int CantidadCompradorProveedor { get; set; }
        public double PrecioCompra { get; set; }
        public DateTime FechaCompra { get; set; }
        public int CantidadDisponible { get; set; }
        public long ArticuloID { get; set; }
        public Articulo Articulo { get; set; }
        public ICollection<EgresoArticulo> EgresosArticulos { get; set; }
        public bool Disabled { get; set; }


    }
}
