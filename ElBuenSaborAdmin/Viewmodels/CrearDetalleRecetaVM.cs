using ElBuenSaborAdmin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSaborAdmin.Viewmodels
{
    public class CrearDetalleRecetaVM
    {
        [Required(ErrorMessage = "{0} es un campo requerido")]
        [Range(1, 1000, ErrorMessage = "Ingrese un número válido")]
        public double Cantidad { get; set; }

        [DisplayName("Artículo")]
        public long? IdArticulo { get; set; }

        [DisplayName("Receta")]
        public long? IdReceta { get; set; }
    }
}
