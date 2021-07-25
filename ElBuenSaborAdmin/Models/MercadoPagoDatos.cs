using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSaborAdmin.Models
{
    public class MercadoPagoDatos
    {
        public long Id { get; set; }
        public long IdentificadorPago { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaAprobacion { get; set; }
        public String FormaPago { get; set; }
        public string MetodoPago { get; set; }
        public String NroTarjeta { get; set; }
        public String Estado { get; set; }
        public long PedidoId { get; set; }
        public Pedido Pedido { get; set; }
        public bool Disabled { get; set; }

    }
}
