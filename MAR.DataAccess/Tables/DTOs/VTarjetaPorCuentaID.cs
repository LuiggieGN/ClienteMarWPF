namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VTarjetaPorCuentaID")]
    public partial class VTarjetaPorCuentaID
    {
        public DateTime? EDiFecha { get; set; }

        [StringLength(50)]
        public string SupNombre { get; set; }

        [Column(TypeName = "money")]
        public decimal? VTarjetas { get; set; }

        public int? RiferoID { get; set; }

        [Key]
        [StringLength(50)]
        public string RifNombre { get; set; }
    }
}
