using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElBuenSaborAdmin.Models;

namespace ElBuenSaborAdmin.Viewmodels
{
    public class IndexStockVM
    {
        public List<Stock> Stocks { get; set; }

        public Articulo Articulo { get; set; }
    }
}
