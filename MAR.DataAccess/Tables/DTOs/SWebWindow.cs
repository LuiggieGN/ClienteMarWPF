namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SWebWindow")]
    public partial class SWebWindow
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int WebWindowID { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(500)]
        public string URL { get; set; }

        public int Activa { get; set; }
    }
}
