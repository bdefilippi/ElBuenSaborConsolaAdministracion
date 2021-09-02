using ElBuenSaborAdmin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSaborAdmin.Viewmodels
{
    public class EditarUsuarioVM
    {
        public Usuario Usuario { get; set; }
        [DisplayName("Reiniciar Contraseña")]
        public bool ReiniciarPass { get; set; }
    }
}
