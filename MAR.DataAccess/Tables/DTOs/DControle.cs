namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DControle
    {
        [Key]
        public int ControlID { get; set; }

        public int GrupoID { get; set; }

        [Required]
        [StringLength(30)]
        public string ConNombre { get; set; }

        public int ConLimite { get; set; }

        public int ConPorUserID { get; set; }

        [Required]
        [StringLength(7)]
        public string ConColor { get; set; }

        [Required]
        [StringLength(1)]
        public string ConQP { get; set; }

        public virtual TGrupos TGrupos { get; set; }
    }
}
