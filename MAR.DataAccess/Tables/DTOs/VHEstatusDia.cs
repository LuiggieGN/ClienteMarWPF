namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VHEstatusDia
    {
        [StringLength(250)]
        public string LotNombre { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EstatusDiaID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EDiFecha { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int GrupoID { get; set; }

        public int? LoteriaID { get; set; }

        public DateTime? EDiInicioVentaFecha { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EDiInicioVentaUserID { get; set; }

        [Key]
        [Column(Order = 3)]
        public bool EDiVentaIniciada { get; set; }

        [Key]
        [Column(Order = 4)]
        public DateTime EDiCierreVentaFecha { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EDiCierreVentaUserID { get; set; }

        [Key]
        [Column(Order = 6)]
        public bool EDiVentaCerrada { get; set; }

        public DateTime? EDiEntradaPremiosFecha { get; set; }

        [Key]
        [Column(Order = 7)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EDiEntradaPremiosUserID { get; set; }

        [Key]
        [Column(Order = 8)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EDiPremiosDentro { get; set; }

        public DateTime? EDiCierreDiaFecha { get; set; }

        [Key]
        [Column(Order = 9)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EDiCierreDiaUserID { get; set; }

        [Key]
        [Column(Order = 10)]
        public bool EDiDiaCerrado { get; set; }

        [Key]
        [Column(Order = 11)]
        public string PremioQ1 { get; set; }

        [Key]
        [Column(Order = 12)]
        [StringLength(1)]
        public string PremioQ2 { get; set; }

        [Key]
        [Column(Order = 13)]
        [StringLength(1)]
        public string PremioQ3 { get; set; }

        public int? EDiClave { get; set; }

        [Key]
        [Column(Order = 14)]
        [StringLength(14)]
        public string Estado { get; set; }

        public string CantidadPremios { get; set; }
        public string CAMPOS_PREMIOS_DROPLIST { get; set; }
    }
}
