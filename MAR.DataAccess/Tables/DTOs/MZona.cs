namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MZona
    {
        [Key]
        public int ZonaID { get; set; }

        [Required]
        [StringLength(150)]
        public string Nombre { get; set; }

        public bool Activa { get; set; }
    }
}
