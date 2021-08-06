using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElBuenSaborAdmin.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ElBuenSaborAdmin.Viewmodels
{
    public class IndexPedidoVM
    {
        public List<Pedido> Pedidos { get; set; }

        public string Estado { get; set; }

        public SelectList SearchString { get; set; }
    }
}
