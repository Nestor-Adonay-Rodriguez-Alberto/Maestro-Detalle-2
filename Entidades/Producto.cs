using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Entidades
{
    public class Producto
    {
        // ATRIBUTOS:

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdProdructo { get; set; }


        [Required(ErrorMessage = "Ingrese Un Nombre Al Producto.")]
        public string Nombre { get; set; }


        [Required(ErrorMessage = "Ingrese El Precio Del Producto.")]
        [Column(TypeName = "decimal(10,2)")]
        public double Precio { get; set; }



        // Tabla Detalle_Factura Asociada A Esta:
        public virtual List<Detalle_Factura> Lista_DetalleFactura { get; set; }


        // Constructor:
        public Producto()
        {
            Lista_DetalleFactura = new List<Detalle_Factura>();
        }
    }
}
