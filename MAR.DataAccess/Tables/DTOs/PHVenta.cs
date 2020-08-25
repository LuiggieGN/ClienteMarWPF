namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PHVenta
    {
        [Key]
        public int VentaID { get; set; }

        public DateTime? VenFecha { get; set; }

        public int? GrupoID { get; set; }

        public int? RiferoID { get; set; }

        public int? BancaID { get; set; }

        public double? BanComision { get; set; }

        public double? SupComision { get; set; }

        public double? SupImpuesto { get; set; }

        public int? SuplidorID { get; set; }

        [StringLength(50)]
        public string SupNombre { get; set; }

        public int? UsuarioID { get; set; }

        [StringLength(50)]
        public string Vendedor { get; set; }

        public double? CTarjetas { get; set; }

        [Column(TypeName = "money")]
        public decimal? VTarjetas { get; set; }
    }
}
