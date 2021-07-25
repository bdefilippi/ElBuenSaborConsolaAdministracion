using ElBuenSaborAdmin.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSaborAdmin.Viewmodels
{
    public class IndexRecetaVM
    {
        public List<Receta> Recetas { get; set; }

        public long? idArticulo { get; set; }
    }
}
