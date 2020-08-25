namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HClaveLocal")]
    public partial class HClaveLocal
    {
        [Key]
        public int ClaveLocalID { get; set; }

        public int BancaID { get; set; }

        [Required]
        [StringLength(10)]
        public string CLoLlave { get; set; }

        [Required]
        [StringLength(10)]
        public string CLoAutoriza { get; set; }

        public int CLoHora { get; set; }

        public DateTime CLoFecha { get; set; }

        [Required]
        [StringLength(15)]
        public string CLoIP { get; set; }

        [Required]
        [StringLength(50)]
        public string CLoUsuario { get; set; }

        public bool CLoHistorico { get; set; }
    }
}
