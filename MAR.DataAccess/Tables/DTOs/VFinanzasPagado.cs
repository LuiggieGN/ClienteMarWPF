namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VFinanzasPagado")]
    public partial class VFinanzasPagado
    {
        [Key]
        [StringLength(9)]
        public string Dia { get; set; }

        public DateTime? EDiFecha { get; set; }

        public int? GrupoID { get; set; }

        [StringLength(50)]
        public string GruNombre { get; set; }

        public int? RiferoID { get; set; }

        [StringLength(50)]
        public string RifNombre { get; set; }

        public int? BancaID { get; set; }

        [StringLength(50)]
        public string BanNombre { get; set; }

        [StringLength(52)]
        public string BanContacto { get; set; }

        public int? LoteriaID { get; set; }

        [StringLength(50)]
        public string LotNombre { get; set; }

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

        public double? CVPales { get; set; }

        public double? CVTripletas { get; set; }

        [Column(TypeName = "money")]
        public decimal? VPales { get; set; }

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

        [Column(TypeName = "money")]
        public decimal? MPagado { get; set; }

        public int? CVTickets { get; set; }
    }
}
