using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSaborAdmin.Models
{
    public class Usuario
    {
        public long Id { get; set; }
        [DisplayName("Nombre de usuario")]
        [Required(ErrorMessage = "{0} es un campo requerido")]
        [StringLength(50, ErrorMessage = "La longitud del {0} debe ser entre {2} y {1} caracteres.", MinimumLength = 2)]
        public String NombreUsuario { get; set; }
        [DisplayName("Contraseña")]
        [Required(ErrorMessage = "{0} es un campo requerido")]
        [StringLength(50, ErrorMessage = "La longitud de la {0} debe ser entre {2} y {1} caracteres.", MinimumLength = 2)]
        public String Clave { get; set; }
        public bool Disabled { get; set; }
        [DisplayName("Rol")]
        public long RolId { get; set; }
        public Rol Rol { get; set; }


    }
}
