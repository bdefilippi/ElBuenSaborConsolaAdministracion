using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSaborAdmin.Viewmodels
{
    public class GananciasVM
    {
        [DisplayName("Fecha inicial")]
        public DateTime FechaInicio { get; set; }
        [DisplayName("Fecha final")]
        public DateTime FechaFinal { get; set; }
    }
}
