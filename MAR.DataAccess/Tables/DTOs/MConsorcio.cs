namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MConsorcio
    {
        [Key]
        public int ConsorcioID { get; set; }

        [Required]
        [StringLength(50)]
        public string ConNombre { get; set; }

        [StringLength(50)]
        public string ConContacto { get; set; }

        [StringLength(50)]
        public string ConTelefonos { get; set; }

        public int ConBancaID { get; set; }

        [Required]
        [StringLength(50)]
        public string ConServidor { get; set; }
    }
}
