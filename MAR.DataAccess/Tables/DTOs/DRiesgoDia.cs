namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DRiesgoDia")]
    public partial class DRiesgoDia
    {
        [Key]
        public int RiesgoID { get; set; }

        public int LoteriaID { get; set; }

        [Required]
        [StringLength(1)]
        public string RieQP { get; set; }

        public double RieMontoVenta { get; set; }

        public double RieComision { get; set; }

        public double RieSaca { get; set; }

        public double RieActual { get; set; }

        public double RieMaximo { get; set; }

        public double RieVenta { get; set; }

        public double RieLimite { get; set; }
    }
}
