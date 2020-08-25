namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HAlerta")]
    public partial class HAlerta
    {
        public int HAlertaID { get; set; }

        public int? AlertaID { get; set; }

        [StringLength(150)]
        public string Entidad { get; set; }

        [StringLength(150)]
        public string Mensaje { get; set; }

        [StringLength(150)]
        public string Usuario { get; set; }

        [StringLength(150)]
        public string Origen { get; set; }

        public int? Nivel { get; set; }

        public DateTime? Fecha { get; set; }
    }
}
