using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSaborAdmin.Models
{
    public class Cliente
    {
        public long Id { get; set; }
        public String Nombre { get; set; }
        public String Apellido { get; set; }
        public long Telefono { get; set; }
        public long UsuarioID { get; set; }
        public Usuario Usuario { get; set; }
        public ICollection<Domicilio> Domicilios { get; set; }
        public ICollection<Pedido> Pedidos { get; set; }
        public bool Disabled { get; set; }
        [NotMapped]
        public string NombreCompleto { get { return this.Nombre + " " + this.Apellido; } }
    }
}
