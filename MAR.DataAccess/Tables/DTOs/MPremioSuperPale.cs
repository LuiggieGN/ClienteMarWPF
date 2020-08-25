namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MPremioSuperPale")]
    public partial class MPremioSuperPale
    {
        [Key]
        public int PremioSuperPaleID { get; set; }

        public int LoteriaIdOrigen1 { get; set; }

        public int LoteriaIdOrigen2 { get; set; }

        public int LoteriaIdDestino { get; set; }

        public bool Activo { get; set; }
    }
}
