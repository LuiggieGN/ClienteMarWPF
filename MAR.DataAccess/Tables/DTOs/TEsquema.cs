namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TEsquema
    {
        [Key]
        public int EsquemaID { get; set; }

        [Required]
        [StringLength(50)]
        public string EsqNombre { get; set; }
    }
}
