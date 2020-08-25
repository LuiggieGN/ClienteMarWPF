namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RF_Loteria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LoteriaID { get; set; }

        [Required]
        [StringLength(250)]
        public string Nombre { get; set; }

        [StringLength(250)]
        public string Referencia { get; set; }

        public bool Activa { get; set; }
    }
}
