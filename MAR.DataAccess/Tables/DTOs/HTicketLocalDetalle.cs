namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HTicketLocalDetalle")]
    public partial class HTicketLocalDetalle
    {
        [Key]
        public int TicketLocalDetalleID { get; set; }

        public int TicketLocalID { get; set; }

        [Required]
        [StringLength(1)]
        public string TLDQP { get; set; }

        [Required]
        [StringLength(6)]
        public string TLDNumero { get; set; }

        public double TLDCantidad { get; set; }

        [Column(TypeName = "money")]
        public decimal TLDCosto { get; set; }

        [Column(TypeName = "money")]
        public decimal TLDPago { get; set; }

        [Required]
        [StringLength(2)]
        public string TLDPagoTipo { get; set; }
    }
}
