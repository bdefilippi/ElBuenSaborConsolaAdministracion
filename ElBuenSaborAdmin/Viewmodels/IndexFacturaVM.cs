using ElBuenSaborAdmin.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSaborAdmin.Viewmodels
{
    public class IndexFacturaVM
    {
        public List<Factura> Facturas { get; set; }

        public SelectList SearchString { get; set; }
    }
}
