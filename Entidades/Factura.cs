using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Entidades
{
    public class Factura
    {
        // ATRIBUTOS:

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdFactura { get; set; }

        [Required(ErrorMessage = "Ingrese La Fecha De Realizada.")]
        public DateTime FechaRealizada { get; set; }

        [Required]
        public int Correlativo { get; set; }

        [Required(ErrorMessage = "Ingrese El Nombre Del Cliente.")]
        public string NombreCliente { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public double Total { get; set; }



        // Tabla Detalle_Factura Asociada A Esta:
        public virtual List<Detalle_Factura> Lista_DetalleFactura { get; set; }

        // Constructor:
        public Factura()
        {
            Lista_DetalleFactura = new List<Detalle_Factura>();  
        }

    }
}
