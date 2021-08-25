using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSaborAdmin.Models
{
    public class DetalleFactura
    {
        public long Id { get; set; }
        [NotMapped]
        public decimal Subtotal { 
            get {
                decimal subtotal = 0;

                subtotal = this.DetallePedido.Cantidad * this.DetallePedido.Articulo.GetUltimoPrecioVenta;

                return subtotal;
            } }
        public bool Disabled { get; set; }
        public long FacturaID { get; set; }
        public Factura Factura { get; set; }    //composicion
        public long DetallePedidoID { get; set; }
        public DetallePedido DetallePedido { get; set; }
        public ICollection<EgresoArticulo> EgresosArticulos { get; set; }
    }
}
