namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MZonaLimite
    {
        [Key]
        public int ZonaLimiteID { get; set; }

        public int ZonaID { get; set; }

        public int LoteriaID { get; set; }

        public double MaxQuiniela { get; set; }

        public double MaxPale { get; set; }

        public double MaxTripleta { get; set; }
    }
}
