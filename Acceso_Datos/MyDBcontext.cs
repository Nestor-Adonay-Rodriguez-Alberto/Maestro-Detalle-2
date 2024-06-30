using Entidades;
using Microsoft.EntityFrameworkCore;


namespace Acceso_Datos
{
    public class MyDBcontext : DbContext
    {
        // Constructor:
        public MyDBcontext(DbContextOptions<MyDBcontext> options)
            : base(options)
        {

        }

        // Tablas En La DB:
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<Detalle_Factura> Detalle_Facturas { get; set; }


    }
}
