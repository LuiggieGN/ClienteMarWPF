namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CL_Pagos
    {
        [Key]
        public int PagoID { get; set; }

        public int BancaID { get; set; }

        public int ReciboID { get; set; }

        [Column(TypeName = "money")]
        public decimal Monto { get; set; }

        [Required]
        [StringLength(100)]
        public string Referencia { get; set; }

        public DateTime Fecha { get; set; }
    }
}
