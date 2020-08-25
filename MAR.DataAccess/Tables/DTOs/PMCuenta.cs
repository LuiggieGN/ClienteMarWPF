namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PMCuenta")]
    public partial class PMCuenta
    {
        [Key]
        public int CuentaID { get; set; }

        [Required]
        [StringLength(50)]
        public string CueNombre { get; set; }

        [Required]
        [StringLength(50)]
        public string CueComercio { get; set; }

        [Column(TypeName = "text")]
        public string CueComentario { get; set; }

        public int CueActiva { get; set; }

        [Required]
        [StringLength(50)]
        public string CueServidor { get; set; }

        public int CuePuerto { get; set; }

        public int? RiferoID { get; set; }

        public string RecargaID { get; set; }
    }
}
