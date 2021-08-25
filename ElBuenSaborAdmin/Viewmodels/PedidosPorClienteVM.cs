using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using ElBuenSaborAdmin.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ElBuenSaborAdmin.Viewmodels
{
    public class PedidosPorClienteVM
    {
        public SelectList Clientes { get; set; }
        [DisplayName("Fecha inicial")]
        public DateTime FechaInicio { get; set; }
        [DisplayName("Fecha final")]
        public DateTime FechaFinal { get; set; }
        [DisplayName("Cliente")]
        public long ClienteID { get; set; }
    }
}
