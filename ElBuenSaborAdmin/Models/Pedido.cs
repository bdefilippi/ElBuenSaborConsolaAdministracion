using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSaborAdmin.Models
{
    public class Pedido
    {
        public long Id { get; set; }
        public long Numero { get; set; }
        public DateTime Fecha { get; set; }
        public int Estado { get; set; }
        public DateTime HoraEstimadaFin { get; set; }
        public int TipoEnvio { get; set; }
        [NotMapped]
        public double Total { get; set; }
        public long ClienteID { get; set; }
        public Cliente Cliente { get; set; }
        public long DomicilioID { get; set; }
        public Domicilio Domicilio { get; set; }
        public bool Disabled { get; set; }
        public ICollection<DetallePedido> DetallesPedido { get; set; }  //composicion
        [NotMapped]
        public string GetEstadoPedido
        {
            get
            {
                string estado = Estado switch
                {
                    1 => "Esperando preparación",
                    2 => "Cocinando",
                    3 => "Pendiente de entrega (Delivery)",
                    4 => "Pendiente de entrega (Local)",
                    5 => "Entregado",
                    6 => "Cancelado",
                    _ => "Esperando aprobación",
                };
                return estado;
            }
        }
        [NotMapped]
        public string GetTipoEnvio { 
            get {
                string estado = Estado switch
                {
                    1 => "Delivery",
                    _ => "Local",
                };
                return estado;
            }
        }

    }
}
