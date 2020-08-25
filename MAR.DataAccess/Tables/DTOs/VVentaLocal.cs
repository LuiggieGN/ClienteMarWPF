namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VVentaLocal")]
    public partial class VVentaLocal
    {
        [Key]
        [Column(Order = 0)]
        public DateTime EDiFecha { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int GrupoID { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RiferoID { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BancaID { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LoteriaID { get; set; }

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

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ResVFuera { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TLoSolicitud { get; set; }
    }
}
