using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSaborAdmin.Viewmodels
{
    public class CrearDomicilioVM
    {
        [Required(ErrorMessage = "{0} es un campo requerido")]
        [StringLength(50, ErrorMessage = "La longitud de la {0} debe ser entre {2} y {1} caracteres.", MinimumLength = 2)]
        public String Calle { get; set; }

        [DisplayName("Número")]
        [Required(ErrorMessage = "{0} es un campo requerido")]
        [Range(1, 99999, ErrorMessage = "Ingrese un número válido")]
        public int Numero { get; set; }

        [Required(ErrorMessage = "{0} es un campo requerido")]
        [StringLength(50, ErrorMessage = "La longitud de la {0} debe ser entre {2} y {1} caracteres.", MinimumLength = 2)]
        public String Localidad { get; set; }

        [DisplayName("Cliente")]
        public long? ClienteID { get; set; }
    }
}
