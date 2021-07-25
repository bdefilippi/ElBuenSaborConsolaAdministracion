using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSaborAdmin.Viewmodels
{
    public class CrearDomicilioVM
    {
        public String Calle { get; set; }
        public int Numero { get; set; }
        public String Localidad { get; set; }
        public long? ClienteID { get; set; }
    }
}
