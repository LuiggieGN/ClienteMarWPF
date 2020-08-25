namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VTarjetaFecBan")]
    public partial class VTarjetaFecBan
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

        [StringLength(50)]
        public string BanContacto { get; set; }

        public double? BanComision { get; set; }

        [Column(TypeName = "money")]
        public decimal? VTarjetas { get; set; }

        public double? CTarjetas { get; set; }

        public double? VTarjComision { get; set; }

        public double? VTarjComisionBanca { get; set; }
    }
}
