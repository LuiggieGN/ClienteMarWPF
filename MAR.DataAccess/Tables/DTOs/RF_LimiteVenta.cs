namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RF_LimiteVenta
    {
        [Key]
        public int LimiteVentaID { get; set; }

        public int? SorteoID { get; set; }

        public int? BancaID { get; set; }

        public int? RiferoID { get; set; }

        public int? ZonaID { get; set; }

        public int? SorteoTipoJugadaID { get; set; }

        [Column(TypeName = "money")]
        public decimal Limite { get; set; }

        public bool SinLimite { get; set; }

        public string Numeros { get; set; }
    }
}
