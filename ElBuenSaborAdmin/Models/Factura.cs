using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSaborAdmin.Models
{
    public class Factura
    {
        public long Id { get; set; }
        [DisplayName("Número")]
        [Required(ErrorMessage = "{0} es un campo requerido")]
        [Range(0, 999999999999, ErrorMessage = "Ingrese un número válido")]
        public long Numero { get; set; }
        public DateTime Fecha { get; set; }
        [DisplayName("Valor Descuento")]
        public decimal MontoDescuento { get; set; }
        public decimal Total { get; set; }
        [NotMapped]
        [DisplayName("Costo Total")]
        public decimal CostoTotal { get {

                decimal total = 0;

                foreach (var detalle in this.DetallesFactura)
                {
                    foreach (var egreso in detalle.EgresosArticulos)
                    {
                        //total usando precio unitario
                        total += egreso.CantidadEgresada * egreso.Stock.GetPrecioUnitario;
                    }
                }
                Console.WriteLine(total);
                return total;
            } }
        public bool Disabled { get; set; }
        public ICollection<DetalleFactura> DetallesFactura { get; set; }    //Es composicion
        [DisplayName("Pedido")]
        public long PedidoId { get; set; }
        public Pedido Pedido { get; set; }
    }
}
