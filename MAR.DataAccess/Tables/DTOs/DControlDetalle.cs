namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DControlDetalle")]
    public partial class DControlDetalle
    {
        [Key]
        public int ControlDetalleID { get; set; }

        public int ControlID { get; set; }

        [Required]
        [StringLength(1)]
        public string CDeQP { get; set; }

        [Required]
        [StringLength(6)]
        public string CDeNumero { get; set; }

        public int RiferoID { get; set; }

        public int LoteriaID { get; set; }
    }
}
