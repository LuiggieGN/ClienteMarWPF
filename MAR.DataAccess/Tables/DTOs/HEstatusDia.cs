namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class HEstatusDia
    {
        [Key]
        public int EstatusDiaID { get; set; }

        public DateTime EDiFecha { get; set; }

        public int GrupoID { get; set; }

        public int LoteriaID { get; set; }

        public DateTime? EDiInicioVentaFecha { get; set; }

        public int EDiInicioVentaUserID { get; set; }

        public bool EDiVentaIniciada { get; set; }

        public DateTime? EDiCierreVentaFecha { get; set; }

        public int EDiCierreVentaUserID { get; set; }

        public bool EDiVentaCerrada { get; set; }

        public DateTime? EDiEntradaPremiosFecha { get; set; }

        public int EDiEntradaPremiosUserID { get; set; }

        public bool EDiPremiosDentro { get; set; }

        public DateTime? EDiCierreDiaFecha { get; set; }

        public int EDiCierreDiaUserID { get; set; }

        public bool EDiDiaCerrado { get; set; }

        [Required]
        [StringLength(2)]
        public string PremioQ1 { get; set; }

        [Required]
        [StringLength(2)]
        public string PremioQ2 { get; set; }

        [Required]
        [StringLength(2)]
        public string PremioQ3 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? EDiClave { get; set; }

        public virtual TGrupos TGrupos { get; set; }
    }
}
