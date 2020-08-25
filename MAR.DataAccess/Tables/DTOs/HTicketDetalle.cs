namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HTicketDetalle")]
    public partial class HTicketDetalle
    {
        public int HTicketDetalleID { get; set; }

        public int TicketDetalleID { get; set; }

        public int TicketID { get; set; }

        [Required]
        [StringLength(1)]
        public string TDeQP { get; set; }

        [Required]
        [StringLength(20)]
        public string TDeNumero { get; set; }

        public double TDeCantidad { get; set; }

        [Column(TypeName = "money")]
        public decimal TDeCosto { get; set; }

        [Column(TypeName = "money")]
        public decimal TDePago { get; set; }

        [Required]
        [StringLength(2)]
        public string TDePagoTipo { get; set; }

        public string TDeLlave { get; set; }
    }
}
