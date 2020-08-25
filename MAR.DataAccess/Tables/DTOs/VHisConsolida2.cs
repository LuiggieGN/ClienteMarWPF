namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VHisConsolida2
    {
        public DateTime? EDiFecha { get; set; }

        public int? RiferoID { get; set; }

        public int? BancaID { get; set; }

        [Key]
        [Column(Order = 0)]
        public double BanComisionQ { get; set; }

        [Key]
        [Column(Order = 1)]
        public double BanComisionP { get; set; }

        [Key]
        [Column(Order = 2)]
        public double BanComisionT { get; set; }

        [Key]
        [Column(Order = 3)]
        public double VTarjComisionBanca { get; set; }

        [Key]
        [Column(Order = 4)]
        public double VTarjComision { get; set; }

        [Key]
        [Column(Order = 5)]
        public double CTarjetas { get; set; }

        [Key]
        [Column(Order = 6, TypeName = "money")]
        public decimal VTarjetas { get; set; }

        [Key]
        [Column(Order = 7)]
        public double CVQuinielas { get; set; }

        [Key]
        [Column(Order = 8, TypeName = "money")]
        public decimal VQuinielas { get; set; }

        [Key]
        [Column(Order = 9)]
        public double CVPales { get; set; }

        [Key]
        [Column(Order = 10, TypeName = "money")]
        public decimal VPales { get; set; }

        [Key]
        [Column(Order = 11)]
        public double CVTripletas { get; set; }

        [Key]
        [Column(Order = 12, TypeName = "money")]
        public decimal VTripletas { get; set; }

        [Key]
        [Column(Order = 13)]
        public double CPrimero { get; set; }

        [Key]
        [Column(Order = 14)]
        public double CSegundo { get; set; }

        [Key]
        [Column(Order = 15)]
        public double CTercero { get; set; }

        [Key]
        [Column(Order = 16)]
        public double CPales { get; set; }

        [Key]
        [Column(Order = 17)]
        public double CTripletas { get; set; }

        [Key]
        [Column(Order = 18, TypeName = "money")]
        public decimal MPrimero { get; set; }

        [Key]
        [Column(Order = 19, TypeName = "money")]
        public decimal MSegundo { get; set; }

        [Key]
        [Column(Order = 20, TypeName = "money")]
        public decimal MTercero { get; set; }

        [Key]
        [Column(Order = 21, TypeName = "money")]
        public decimal MPales { get; set; }

        [Key]
        [Column(Order = 22, TypeName = "money")]
        public decimal MTripletas { get; set; }

        [Key]
        [Column(Order = 23)]
        public double RifDescuento { get; set; }

        [Key]
        [Column(Order = 24, TypeName = "money")]
        public decimal ISRRetenido { get; set; }

        [Key]
        [Column(Order = 25, TypeName = "money")]
        public decimal PagoRemoto { get; set; }
    }
}
