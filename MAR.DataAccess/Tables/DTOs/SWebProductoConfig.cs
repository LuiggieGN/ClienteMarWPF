namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SWebProductoConfig")]
    public partial class SWebProductoConfig
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int WebProductoConfigID { get; set; }

        public int WebProductoID { get; set; }

        [Required]
        [StringLength(50)]
        public string Opcion { get; set; }

        [Required]
        [StringLength(1000)]
        public string Valor { get; set; }

        public int Modo { get; set; }

        public bool Activo { get; set; }
    }
}
