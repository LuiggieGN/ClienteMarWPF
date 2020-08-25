namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VP_LimiteVenta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LimiteVentaID { get; set; }

        public int? SuplidorID { get; set; }

        public int? BancaID { get; set; }

        public int? RiferoID { get; set; }

        public int? ZonaID { get; set; }

        public int? ProductoID { get; set; }

        [Column(TypeName = "money")]
        public decimal Limite { get; set; }

        public bool SinLimite { get; set; }
    }
}
