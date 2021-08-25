using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSaborAdmin.Models
{
    public class Cliente
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "{0} es un campo requerido")]
        [StringLength(50, ErrorMessage = "La longitud del {0} debe ser entre {2} y {1} caracteres.", MinimumLength = 2)]
        public String Nombre { get; set; }
        [Required(ErrorMessage = "{0} es un campo requerido")]
        [StringLength(50, ErrorMessage = "La longitud del {0} debe ser entre {2} y {1} caracteres.", MinimumLength = 2)]
        public String Apellido { get; set; }
        [DisplayName("Teléfono")]
        [Required(ErrorMessage = "{0} es un campo requerido")]
        [RegularExpression(@"^\d{6,11}$", ErrorMessage = "Por favor ingrese un número válido (Entre 6 y 11 dígitos)")]
        public long Telefono { get; set; }
        [DisplayName("Usuario")]
        public long UsuarioID { get; set; }
        public Usuario Usuario { get; set; }
        public ICollection<Domicilio> Domicilios { get; set; }
        public ICollection<Pedido> Pedidos { get; set; }
        public bool Disabled { get; set; }
        [NotMapped]
        public string NombreCompleto { get { return this.Nombre + " " + this.Apellido; } }
    }
}
