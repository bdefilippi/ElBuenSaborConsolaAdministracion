using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ElBuenSaborAdmin.Models;

namespace ElBuenSaborAdmin.Viewmodels
{
    public class CrearStockVM
    {
        [DisplayName("Cant. Comprada")]
        [Required(ErrorMessage = "{0} es un campo requerido")]
        [Range(0, 99999999, ErrorMessage = "Ingrese un valor válido")]
        public int CantidadCompradorProveedor { get; set; }
        [DisplayName("Precio de compra")]
        [Required(ErrorMessage = "{0} es un campo requerido")]
        [Range(0, 99999999.99, ErrorMessage = "Ingrese un valor válido")]
        public decimal PrecioCompra { get; set; }
        [DisplayName("Fecha")]
        [Required(ErrorMessage = "{0} es un campo requerido")]
        public DateTime FechaCompra { get; set; }
        [DisplayName("Cant. Disponible")]
        public int CantidadDisponible { get; set; }
        [DisplayName("Artículo")]
        public long ArticuloID { get; set; }

    }
}
