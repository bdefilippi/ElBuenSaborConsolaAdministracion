using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using ElBuenSaborAdmin.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ElBuenSaborAdmin.Viewmodels
{
    public class PedidosPorClienteVM
    {
        [Required]
        public SelectList Clientes { get; set; }
        [Required]
        [DisplayName("Fecha inicial")]
        public DateTime FechaInicio { get; set; }
        [Required]
        [DisplayName("Fecha final")]
        public DateTime FechaFinal { get; set; }
        [Required]
        [DisplayName("Cliente")]
        public long ClienteID { get; set; }
    }
}
