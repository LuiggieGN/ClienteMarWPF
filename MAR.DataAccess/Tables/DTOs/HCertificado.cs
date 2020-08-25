namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HCertificados")]
    public partial class HCertificado
    {
        [Key]
        public int CertificadoID { get; set; }

        public int CerNumero { get; set; }

        public int BancaID { get; set; }

        public int CerHwKey { get; set; }

        public DateTime CerFecha { get; set; }
    }
}
