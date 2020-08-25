namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CL_ClienteDetalle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ClienteDetalleID { get; set; }

        public int ClienteID { get; set; }

        public int ClienteCampoID { get; set; }

        [Required]
        public string Referencia { get; set; }

        public string ValorText { get; set; }

        public double? ValorMonto { get; set; }
    }
}
