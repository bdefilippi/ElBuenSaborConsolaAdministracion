using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSaborAdmin.Models
{
    public class RubroArticulo
    {
        public long Id { get; set; }
        [DisplayName("Denominación")]
        [Required(ErrorMessage = "{0} es un campo requerido")]
        [StringLength(50, ErrorMessage = "La longitud de la {0} debe ser entre {2} y {1} caracteres.", MinimumLength = 2)]
        public String Denominacion { get; set; }
        public bool Disabled { get; set; }
        public ICollection<Articulo> Articulos { get; set; }

    }
}
