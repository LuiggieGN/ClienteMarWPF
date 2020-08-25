namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DAlerta")]
    public partial class DAlerta
    {
        [Key]
        public int AlertaID { get; set; }

        [Required]
        [StringLength(150)]
        public string Entidad { get; set; }

        [Required]
        [StringLength(150)]
        public string Mensaje { get; set; }

        [Required]
        [StringLength(150)]
        public string Usuario { get; set; }

        [Required]
        [StringLength(150)]
        public string Origen { get; set; }

        public int Nivel { get; set; }

        public DateTime Fecha { get; set; }
    }
}
