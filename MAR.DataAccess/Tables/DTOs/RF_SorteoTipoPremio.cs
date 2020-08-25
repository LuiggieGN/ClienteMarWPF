namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RF_SorteoTipoPremio
    {
        [Key]
        public int SorteoTipoPremioID { get; set; }

        public int SorteoTipoID { get; set; }

        public int LogicaKey { get; set; }

        [Required]
        [StringLength(250)]
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public bool Activo { get; set; }
    }
}
