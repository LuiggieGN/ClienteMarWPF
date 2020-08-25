namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RF_SorteoCampo
    {
        [Key]
        public int SorteoCampoID { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Referencia { get; set; }

        [Required]
        [StringLength(20)]
        public string TipoDato { get; set; }

        public string OpcionesReferencias { get; set; }

        public string OpcionesNombres { get; set; }

        public bool Activo { get; set; }

        public int? SorteoID { get; set; }

        public virtual RF_Sorteo RF_Sorteo { get; set; }
    }
}
