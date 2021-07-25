using ElBuenSaborAdmin.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSaborAdmin.Viewmodels
{
    public class CrearRecetaVM
    {
        public int TiempoEstimadoCocina { get; set; }
        public string Descripcion { get; set; }
        public long? ArticuloID { get; set; }

    }
}
