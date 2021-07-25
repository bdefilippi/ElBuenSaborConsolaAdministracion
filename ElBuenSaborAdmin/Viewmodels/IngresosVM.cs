using ElBuenSaborAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ElBuenSaborAdmin.Viewmodels
{
    public class IngresosVM
    {
        public List<Stock> Stocks { get; set; }
        public SelectList Articulos { get; set; }
        public SelectList ArticulosNombre { get; set; }
    }
}
