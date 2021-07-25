using ElBuenSaborAdmin.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSaborAdmin.Viewmodels
{
    public class DomicilioClienteVM
    {
        public List<Domicilio> Domicilios { get; set; }
        public SelectList Clientes { get; set; }
        public SelectList ClientesNombres { get; set; }

    }
}
