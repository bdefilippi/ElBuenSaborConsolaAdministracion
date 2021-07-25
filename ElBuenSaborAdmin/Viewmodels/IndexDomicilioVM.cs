using ElBuenSaborAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSaborAdmin.Viewmodels
{
    public class IndexDomicilioVM
    {
        public List<Domicilio> Domicilios { get; set; }

        public long? ClienteID { get; set; }
    }
}
