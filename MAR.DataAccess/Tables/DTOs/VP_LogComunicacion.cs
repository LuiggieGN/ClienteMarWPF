namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VP_LogComunicacion
    {
        [Key]
        public int LogComunicacionID { get; set; }

        public int CuentaID { get; set; }

        public int? PinID { get; set; }

        public int? TransaccionID { get; set; }

        [Required]
        public string Origen { get; set; }

        [Required]
        public string Destino { get; set; }

        [Required]
        public string DataEnviada { get; set; }

        public string DataRecibida { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; }

        public DateTime Fecha { get; set; }
    }
}
