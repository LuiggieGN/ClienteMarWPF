namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VentaConPuto")]
    public partial class VentaConPuto
    {
        [Key]
        [Column(Order = 0)]
        public int TicketDetalleID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TicketID { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(1)]
        public string TDeQP { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(6)]
        public string TDeNumero { get; set; }

        [Key]
        [Column(Order = 4)]
        public double TDeCantidad { get; set; }

        [Key]
        [Column(Order = 5, TypeName = "money")]
        public decimal TDeCosto { get; set; }

        [Key]
        [Column(Order = 6, TypeName = "money")]
        public decimal TDePago { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(2)]
        public string TDePagoTipo { get; set; }
    }
}
