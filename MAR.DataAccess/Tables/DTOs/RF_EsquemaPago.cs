namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RF_EsquemaPago
    {
        [Key]
        public int EsquemaPagoID { get; set; }

        public int SorteoTipoID { get; set; }

        [Required]
        [StringLength(250)]
        public string Nombre { get; set; }

        public string Description { get; set; }
    }
}
