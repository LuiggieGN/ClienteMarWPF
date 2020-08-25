namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class HResuman
    {
        [Key]
        public int ResumenID { get; set; }

        public DateTime? EDiFecha { get; set; }

        public int? GrupoID { get; set; }

        public int? RiferoID { get; set; }

        public int? BancaID { get; set; }

        public int? LoteriaID { get; set; }

        public double? BanComisionQ { get; set; }

        public double? BanComisionP { get; set; }

        public double? BanComisionT { get; set; }

        [StringLength(2)]
        public string Primero { get; set; }

        [StringLength(2)]
        public string Segundo { get; set; }

        [StringLength(2)]
        public string Tercero { get; set; }

        public double? CVQuinielas { get; set; }

        [Column(TypeName = "money")]
        public decimal? VQuinielas { get; set; }

        public double? CVBilletes { get; set; }

        [Column(TypeName = "money")]
        public decimal? VBilletes { get; set; }

        public double? CVPales { get; set; }

        [Column(TypeName = "money")]
        public decimal? VPales { get; set; }

        public double? CVTripletas { get; set; }

        [Column(TypeName = "money")]
        public decimal? VTripletas { get; set; }

        public double? CPrimero { get; set; }

        public double? CSegundo { get; set; }

        public double? CTercero { get; set; }

        public double? CPales { get; set; }

        public double? CTripletas { get; set; }

        [Column(TypeName = "money")]
        public decimal? MPrimero { get; set; }

        [Column(TypeName = "money")]
        public decimal? MSegundo { get; set; }

        [Column(TypeName = "money")]
        public decimal? MTercero { get; set; }

        [Column(TypeName = "money")]
        public decimal? MPales { get; set; }

        [Column(TypeName = "money")]
        public decimal? MTripletas { get; set; }

        public bool ResVFuera { get; set; }

        [Column(TypeName = "money")]
        public decimal? MPagado { get; set; }

        public int? CVTickets { get; set; }

        public DateTime ResCierre { get; set; }

        public double? RifDescuento { get; set; }

        [Column(TypeName = "money")]
        public decimal? ISRRetenido { get; set; }
    }
}
