using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSaborAdmin.Viewmodels
{
    public class GananciasVM
    {
        [Required]
        [DisplayName("Fecha inicial")]
        public DateTime FechaInicio { get; set; }
        [Required]
        [DisplayName("Fecha final")]
        public DateTime FechaFinal { get; set; }
    }
}
