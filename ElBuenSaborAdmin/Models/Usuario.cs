﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSaborAdmin.Models
{
    public class Usuario
    {
        public long Id { get; set; }
        public String NombreUsuario { get; set; }
        public String Clave { get; set; }
        public bool Disabled { get; set; }
        public long RolId { get; set; }
        public Rol Rol { get; set; }


    }
}
