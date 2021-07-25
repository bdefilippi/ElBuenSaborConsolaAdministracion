using ElBuenSaborAdmin.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElBuenSaborAdmin.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Articulo> Articulos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Configuracion> Configuraciones { get; set; }
        public DbSet<DetalleFactura> DetallesFacturas { get; set; }
        public DbSet<DetallePedido> DetallesPedidos { get; set; }
        public DbSet<DetalleReceta> DetallesRecetas { get; set; }
        public DbSet<Domicilio> Domicilios { get; set; }
        public DbSet<EgresoArticulo> EgresosArticulos { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<MercadoPagoDatos> MercadoPagoDatos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PrecioVentaArticulo> PreciosVentaArticulos { get; set; }
        public DbSet<Receta> Recetas { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<RubroArticulo> RubrosArticulos { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
