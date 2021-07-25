using ElBuenSaborAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSaborAdmin.Viewmodels
{
    public class CrearDetalleRecetaVM
    {
        public double Cantidad { get; set; }
        public long? IdArticulo { get; set; }
        public long? IdReceta { get; set; }
    }
}
