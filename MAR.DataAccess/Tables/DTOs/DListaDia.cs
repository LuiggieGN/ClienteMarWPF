namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DListaDia")]
    public partial class DListaDia
    {
        [Key]
        public int ListaDiaID { get; set; }

        [Required]
        [StringLength(6)]
        public string LDiNumero { get; set; }

        public int LoteriaID { get; set; }

        public int BancaID { get; set; }

        public double LDiCantidad { get; set; }

        [Required]
        [StringLength(1)]
        public string LDiQP { get; set; }

        public int RiferoID { get; set; }

        public int GrupoID { get; set; }
    }
}
