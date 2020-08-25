namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SReporte
    {
        [Key]
        public int ReporteID { get; set; }

        [Required]
        [StringLength(250)]
        public string RepNombre { get; set; }

        [Required]
        [StringLength(50)]
        public string RepFuente { get; set; }

        [Required]
        [StringLength(50)]
        public string RepArchivo { get; set; }

        public int RepListOrden { get; set; }

        public bool RepFecha { get; set; }

        public bool RepRango { get; set; }

        public bool RepLoteria { get; set; }

        public bool RepRifero { get; set; }

        public bool RepBanca { get; set; }

        public bool RepTipo { get; set; }

        [Required]
        [StringLength(50)]
        public string RepOrder { get; set; }
    }
}
