namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VTicketsDia")]
    public partial class VTicketsDia
    {
        [StringLength(15)]
        public string TicNumero { get; set; }

        public DateTime? TicFecha { get; set; }

        [Column(TypeName = "money")]
        public decimal? TicCosto { get; set; }

        public double? CQuinielas { get; set; }

        public double? CPales { get; set; }

        public double? CPrimera { get; set; }

        public double? CSegunda { get; set; }

        public double? CTercera { get; set; }

        [Column(TypeName = "money")]
        public decimal? MPales { get; set; }

        [Column(TypeName = "money")]
        public decimal? Saco { get; set; }

        [Key]
        [Column(Order = 0)]
        public bool TicNulo { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BancaID { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RiferoID { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LoteriaID { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int GrupoID { get; set; }

        [StringLength(50)]
        public string BanContacto { get; set; }

        [StringLength(50)]
        public string LotNombre { get; set; }

        [StringLength(50)]
        public string GruNombre { get; set; }

        [StringLength(2)]
        public string PremioQ1 { get; set; }

        [StringLength(2)]
        public string PremioQ2 { get; set; }

        [StringLength(2)]
        public string PremioQ3 { get; set; }

        public DateTime? EDiFecha { get; set; }

        [Key]
        [Column(Order = 5)]
        public bool TicPagado { get; set; }
    }
}
