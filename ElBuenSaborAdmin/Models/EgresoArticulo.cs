using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSaborAdmin.Models
{
    public class EgresoArticulo
    {
        public long Id { get; set; }

        [DisplayName("Cantidad Egresada")]
        [Required(ErrorMessage = "{0} es un campo requerido")]
        [Range(1, 99999, ErrorMessage = "Ingrese un número válido")]
        public int CantidadEgresada { get; set; }

        [DisplayName("Stock")]
        public long StockID { get; set; }
        public Stock Stock { get; set; }

        [DisplayName("Detalle Factura")]
        public long DetalleFacturaId { get; set; }
        public DetalleFactura DetalleFactura { get; set; }
        public bool Disabled { get; set; }

    }
}
