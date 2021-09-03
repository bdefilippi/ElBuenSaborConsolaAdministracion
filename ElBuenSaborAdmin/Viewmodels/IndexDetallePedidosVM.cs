using ElBuenSaborAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSaborAdmin.Viewmodels
{
    public class IndexDetallePedidosVM
    {
        public List<DetallePedido> DetallesPedido { get; set; }

        public long? IdPedido { get; set; }

    }
}
