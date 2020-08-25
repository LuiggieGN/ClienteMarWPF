namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HImpuestoRetenido")]
    public partial class HImpuestoRetenido
    {
        public int HImpuestoRetenidoID { get; set; }

        public int ImpuestoRetenidoID { get; set; }

        public int TicketID { get; set; }

        public DateTime Fecha { get; set; }

        public int BancaID { get; set; }

        [Column(TypeName = "money")]
        public decimal TotalSaco { get; set; }

        [Column(TypeName = "money")]
        public decimal Retenido { get; set; }

        [Required]
        [StringLength(15)]
        public string TicNumero { get; set; }

        public int TipoImpuestoID { get; set; }
    }
}
