using ElBuenSaborAdmin.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSaborAdmin.Viewmodels
{
    public class IndexArticuloVM
    {
        public List<Articulo> Articulos { get; set; }
        public SelectList Rubros { get; set; }
        public SelectList RubrosNombres { get; set; }
        public String Rubro { get; set; }
        public String SearchString { get; set; }
    }
}
