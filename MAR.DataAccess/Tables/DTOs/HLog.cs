namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HLog")]
    public partial class HLog
    {
        [Key]
        public int LogID { get; set; }

        [Required]
        [StringLength(50)]
        public string LogTipo { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string LogComentario { get; set; }

        public DateTime LogFecha { get; set; }

        [Required]
        [StringLength(50)]
        public string UsuarioID { get; set; }

        [Required]
        [StringLength(15)]
        public string SecRemoteIP { get; set; }

        public int BancaID { get; set; }
    }
}
