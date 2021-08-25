using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSaborAdmin.Models
{
    public class Domicilio
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "{0} es un campo requerido")]
        [StringLength(50, ErrorMessage = "La longitud de la {0} debe ser entre {2} y {1} caracteres.", MinimumLength = 2)]
        public String Calle { get; set; }
        [DisplayName("Número")]
        [Required(ErrorMessage = "{0} es un campo requerido")]
        public int Numero { get; set; }
        [Required(ErrorMessage = "{0} es un campo requerido")]
        [StringLength(50, ErrorMessage = "La longitud de la {0} debe ser entre {2} y {1} caracteres.", MinimumLength = 2)]
        public String Localidad { get; set; }
        public bool Disabled { get; set; }
        public ICollection<Pedido> Pedidos { get; set; }
        [DisplayName("Cliente")]
        public long ClienteID { get; set; }
        public Cliente Cliente { get; set; }

        [NotMapped]
        public String GetDomicilioCompleto { 
            get {
                return this.Calle + " " + this.Numero + " - " + this.Localidad;
            } }
    }
}
