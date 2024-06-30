using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Entidades
{
    public class Detalle_Factura
    {
        // ATRIBUTOS:

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdDetalleFactura { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public double PrecioDel_Producto { get; set; }



        // Referencia Tabla Producto:
        [ForeignKey("Objeto_Producto")]
        public int IdProductoEnDetalle { get; set; }
        public virtual Producto Objeto_Producto { get; set; }


        // Referencia Tabla Factura:
        [ForeignKey("Objeto_Factura")]
        public int IdFacturaEnDetalle { get; set; }
        public virtual Factura Objeto_Factura { get; set; }

    }
}
