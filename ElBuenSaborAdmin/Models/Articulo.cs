using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSaborAdmin.Models
{
    public class Articulo
    {
        public long Id { get; set; }
        [DisplayName("Denominación")]
        [Required(ErrorMessage = "{0} es un campo requerido")]
        [StringLength(50, ErrorMessage = "La longitud de la {0} debe ser entre {2} y {1} caracteres.", MinimumLength = 2)]
        public String Denominacion { get; set; }
        public String Imagen { get; set; }
        [DisplayName("Imagen")]
        [Required(ErrorMessage = "{0} es un campo requerido")]
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        [NotMapped]
        public string ImageSrc { get; set; }
        [DisplayName("Unidad de medida")]
        [Required(ErrorMessage = "{0} es un campo requerido")]
        [StringLength(50, ErrorMessage = "La longitud de la {0} debe ser entre {2} y {1} caracteres.", MinimumLength = 1)]
        public String UnidadMedida { get; set; }
        [DisplayName("Stock mínimo")]
        [Required(ErrorMessage = "{0} es un campo requerido")]
        [Range(0, 9999999, ErrorMessage = "Ingrese un número válido")]
        public double StockMinimo { get; set; }
        [NotMapped]
        [DisplayName("Stock actual")]
        public double StockActual { get; set; }
        [DisplayName("Producto a la venta")]
        [Required(ErrorMessage = "{0} es un campo requerido")]
        public bool ALaVenta { get; set; }
        public bool Disabled { get; set; }
        [DisplayName("Rubro")]
        [Required(ErrorMessage = "{0} es un campo requerido")]
        public long RubroArticuloID { get; set; }
        public RubroArticulo RubroArticulo { get; set; }
        public ICollection<PrecioVentaArticulo> PreciosVentaArticulos { get; set; }
        public ICollection<Receta> Recetas { get; set; }
        public ICollection<DetalleReceta> DetallesRecetas { get; set; }
        public ICollection<Stock> Stocks { get; set; }
        public ICollection<DetallePedido> DetallesPedidos { get; set; }
        [DisplayName("Es artículo manufacturado")]
        [Required(ErrorMessage = "{0} es un campo requerido")]
        public bool EsManufacturado { get; set; }

        [NotMapped]
        public string GetDenominacionConUnidad { get { return this.Denominacion + " (" + this.UnidadMedida + ")"; } }
        [NotMapped]
        public decimal GetUltimoPrecioVenta { 
            get {
                decimal precioVenta = 0;
                try
                {
                    if (this.ALaVenta)
                    {
                        precioVenta = this.PreciosVentaArticulos.Where(p => p.Disabled.Equals(false)).OrderByDescending(p => p.Fecha).FirstOrDefault().PrecioVenta;
                    }  
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                    precioVenta = 0;
                }

                return precioVenta;
            } }

    }
}
