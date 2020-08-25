namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class HControle
    {
        [Key]
        public int HControlID { get; set; }

        public int ControlID { get; set; }

        public int LoteriaID { get; set; }

        public int GrupoID { get; set; }

        [Required]
        [StringLength(30)]
        public string ConNombre { get; set; }

        public int ConLimite { get; set; }

        public int ConPorUserID { get; set; }

        public int RiferoID { get; set; }

        public DateTime ConFecha { get; set; }

        public virtual TGrupos TGrupos { get; set; }
    }
}
