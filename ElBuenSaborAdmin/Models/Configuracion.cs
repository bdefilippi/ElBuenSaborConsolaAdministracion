using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSaborAdmin.Models
{
    public class Configuracion
    {

        public long Id { get; set; }
        [Required(ErrorMessage = "{0} es un campo requerido")]
        [Range(1, 1000, ErrorMessage = "Ingrese un número válido")]
        [DisplayName("Cantidad de Cocineros")]
        public int CantidadCocineros { get; set; }
        [DisplayName("Email de la Empresa")]
        [Required(ErrorMessage = "{0} es un campo requerido")]
        [EmailAddress(ErrorMessage = "Ingrese una dirección de correo válida")]
        public String EmailEmpresa { get; set; }
        [DisplayName("Token de Mercado Pago")]
        [Required(ErrorMessage = "{0} es un campo requerido")]
        public String TokenMercadoPago { get; set; }
        public bool Disabled { get; set; }


    }
}
