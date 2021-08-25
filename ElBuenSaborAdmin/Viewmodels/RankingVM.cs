using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSaborAdmin.Viewmodels
{
    public class RankingVM
    {
        [DisplayName("Fecha inicial")]
        public DateTime FechaInicio { get; set; }
        [DisplayName("Fecha final")]
        public DateTime FechaFinal { get; set; }
    }
}
