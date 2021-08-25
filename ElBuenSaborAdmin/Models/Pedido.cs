using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSaborAdmin.Models
{
    public class Pedido
    {
        public long Id { get; set; }
        [DisplayName("Precio")]
        public long Numero { get; set; }
        public DateTime Fecha { get; set; }
        public int Estado { get; set; }
        [DisplayName("Hora estimada de entrega")]
        public DateTime HoraEstimadaFin { get; set; }
        [DisplayName("Retira")]
        public int TipoEnvio { get; set; }
        [NotMapped]
        public double Total { get; set; }
        [DisplayName("Precio")]
        public long ClienteID { get; set; }
        public Cliente Cliente { get; set; }
        [DisplayName("Precio")]
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
        [NotMapped]
        public decimal GetTotal
        {
            get
            {
                decimal total = 0;
                foreach (var detalle in this.DetallesPedido)
                {
                    total += detalle.GetTotal;
                }
                return total;
            }
        }

    }
}
