namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VHisGrafico")]
    public partial class VHisGrafico
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string BanContacto { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string RifNombre { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string LotNombre { get; set; }

        public DateTime? EdiFecha { get; set; }

        public int? BancaID { get; set; }

        public int? LoteriaID { get; set; }

        public int? RiferoID { get; set; }

        [Column(TypeName = "money")]
        public decimal? Ventas { get; set; }

        public double? Beneficio { get; set; }

        [Column(TypeName = "money")]
        public decimal? Saco { get; set; }
    }
}
