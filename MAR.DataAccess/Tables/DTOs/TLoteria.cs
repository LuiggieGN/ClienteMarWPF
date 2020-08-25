namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TLoteria
    {
        [Key]
        public int LoteriaID { get; set; }

        [Required]
        [StringLength(50)]
        public string LotNombre { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string LotComentario { get; set; }

        public bool LotActivo { get; set; }

        public bool LotOculta { get; set; }

        [StringLength(20)]
        public string NombreResumido { get; set; }

        [StringLength(100)]
        public string Imagen { get; set; }
    }
}
