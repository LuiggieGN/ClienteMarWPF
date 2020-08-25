namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HSecurityLog")]
    public partial class HSecurityLog
    {
        [Key]
        public int SecurityLogID { get; set; }

        public int LogID { get; set; }

        [Required]
        [StringLength(50)]
        public string SecTipo { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string SecComentario { get; set; }

        public DateTime SecFecha { get; set; }

        [Required]
        [StringLength(50)]
        public string UsuarioID { get; set; }

        [Required]
        [StringLength(15)]
        public string SecRemoteIP { get; set; }

        public int BancaID { get; set; }
    }
}
