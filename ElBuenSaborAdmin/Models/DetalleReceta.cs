using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSaborAdmin.Models
{
    public class DetalleReceta
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "{0} es un campo requerido")]
        [Range(1, 1000, ErrorMessage = "Ingrese un número válido")]
        public double Cantidad { get; set; }
        [DisplayName("Artículo")]
        public long ArticuloID { get; set; }
        public Articulo Articulo { get; set; }
        [DisplayName("Receta")]
        public long RecetaID { get; set; }
        public Receta Receta { get; set; }
        public bool Disabled { get; set; }

        [NotMapped]
        public decimal GetCosto
        {
            get
            {
                decimal cantidad = (decimal)this.Cantidad;
                decimal subtotal;
                try
                {
                    subtotal = this.Articulo.Stocks.OrderBy(s => s.FechaCompra).Where(s => s.Disabled != true && s.CantidadDisponible > 0).First().GetPrecioUnitario;
                }
                catch (Exception e)
                {
                    subtotal = 0;
                    Console.WriteLine("Error en la edicion", e);
                }
                

                return cantidad * subtotal;
            }
        }

    }
}
