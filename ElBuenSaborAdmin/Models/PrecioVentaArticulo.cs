using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSaborAdmin.Models
{
    public class PrecioVentaArticulo
    {
        public long Id { get; set; }
        [DisplayName("Precio")]
        [Required(ErrorMessage = "{0} es un campo requerido")]
        [Range(0, 999999999 ,ErrorMessage = "Ingrese un número válido")]
        public decimal PrecioVenta { get; set; }
        [Required(ErrorMessage = "{0} es un campo requerido")]
        public DateTime Fecha { get; set; }
        [DisplayName("Artículo")]
        public long ArticuloID { get; set; }
        public Articulo Articulo { get; set; }
        public bool Disabled { get; set; }


    }
}
