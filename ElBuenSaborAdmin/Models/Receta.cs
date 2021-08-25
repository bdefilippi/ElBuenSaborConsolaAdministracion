using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSaborAdmin.Models
{
    public class Receta
    {
        public long Id { get; set; }
        [DisplayName("Tiempo estimado de cocina")]
        [Required(ErrorMessage = "{0} es un campo requerido")]
        [Range(0, 9999, ErrorMessage ="Ingrese un valor válido")]
        public int TiempoEstimadoCocina { get; set; }
        [DisplayName("Descripción")]
        [Required(ErrorMessage = "{0} es un campo requerido")]
        public string Descripcion { get; set; }
        public bool Disabled { get; set; }
        [DisplayName("Artículo")]
        public long ArticuloID { get; set; }
        public Articulo Articulo { get; set; }
        public ICollection<DetalleReceta> DetallesRecetas { get; set; }
    }
}
