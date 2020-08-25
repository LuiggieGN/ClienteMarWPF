namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HContabilidad")]
    public partial class HContabilidad
    {
        [Key]
        public int ContabilidadID { get; set; }

        public int? CuentaID { get; set; }

        [StringLength(50)]
        public string CueNombre { get; set; }

        [StringLength(11)]
        public string CueNumero { get; set; }

        public int BancaID { get; set; }

        public DateTime ConFecha { get; set; }
    }
}
