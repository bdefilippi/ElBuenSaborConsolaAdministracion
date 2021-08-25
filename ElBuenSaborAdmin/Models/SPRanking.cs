using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSaborAdmin.Models
{
    [Keyless]
    public class SPRanking
    {
        public int Cantidad { get; set; }
        public string Denominacion { get; set; }
    }
}
