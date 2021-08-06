using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSaborAdmin.Models
{
    public class Factura
    {
        public long Id { get; set; }
        public long Numero { get; set; }
        public DateTime Fecha { get; set; }
        public double MontoDescuento { get; set; }
        public String FormaPago { get; set; }
        public decimal Total { get; set; }
        //[NotMapped]
        //public double TotalVenta { 
        //    get {
        //        double total = 0;

        //        foreach (var detalle in this.DetallesFactura)
        //        {
        //            total += detalle.Subtotal;
        //        }

        //        return total;
        //    } }
        [NotMapped]
        public decimal TotalCosto { get; set; }
        public bool Disabled { get; set; }
        public ICollection<DetalleFactura> DetallesFactura { get; set; }    //Es composicion
        public long PedidoId { get; set; }
        public Pedido Pedido { get; set; }
    }
}
