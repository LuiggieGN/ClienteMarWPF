namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MCuenta
    {
        [Key]
        public int CuentaID { get; set; }

        [Required]
        [StringLength(50)]
        public string CueNombre { get; set; }

        [Required]
        [StringLength(11)]
        public string CueNumero { get; set; }

        [Column(TypeName = "text")]
        public string CueComentario { get; set; }

        public bool CueActiva { get; set; }
    }
}
