namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MBancasConfig")]
    public partial class MBancasConfig
    {
        [Key]
        public int BancaConfigID { get; set; }

        public int BancaID { get; set; }

        [Required]
        [StringLength(200)]
        public string ConfigKey { get; set; }

        public string ConfigValue { get; set; }

        public bool Activo { get; set; }
    }
}
