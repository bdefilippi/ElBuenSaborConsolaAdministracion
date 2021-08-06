using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSaborAdmin.Models
{
    public class Domicilio
    {
        public long Id { get; set; }
        public String Calle { get; set; }
        public int Numero { get; set; }
        public String Localidad { get; set; }
        public bool Disabled { get; set; }
        public ICollection<Pedido> Pedidos { get; set; }
        public long ClienteID { get; set; }
        public Cliente Cliente { get; set; }

        [NotMapped]
        public String GetDomicilioCompleto { 
            get {
                return this.Calle + " " + this.Numero + " - " + this.Localidad;
            } }
    }
}
