namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MImpuestoRango")]
    public partial class MImpuestoRango
    {
        [Key]
        public int ImpuestoRangoID { get; set; }

        public DateTime FechaEfectividad { get; set; }

        public DateTime? FechaExpira { get; set; }

        [Column(TypeName = "money")]
        public decimal DesdeMonto { get; set; }

        [Column(TypeName = "money")]
        public decimal HastaMonto { get; set; }

        public double PorcientoRetencion { get; set; }

        public bool Activo { get; set; }

        public int TipoImpuestoID { get; set; }
    }
}
