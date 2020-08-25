namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VTransacionFecBan")]
    public partial class VTransacionFecBan
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(9)]
        public string Dia { get; set; }

        public DateTime? EDiFecha { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int GrupoID { get; set; }

        [StringLength(50)]
        public string GruNombre { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RiferoID { get; set; }

        [StringLength(50)]
        public string RifNombre { get; set; }

        public int? BancaID { get; set; }

        [StringLength(50)]
        public string BanNombre { get; set; }

        [StringLength(50)]
        public string BanContacto { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BanComision { get; set; }

        [Column(TypeName = "money")]
        public decimal? VTarjetas { get; set; }

        [Column(TypeName = "money")]
        public decimal? CTarjetas { get; set; }

        [Column(TypeName = "money")]
        public decimal? VTarjComision { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int VTarjComisionBanca { get; set; }

        [StringLength(20)]
        public string Referencia { get; set; }

        public int? ProductoID { get; set; }

        public string ProdNombre { get; set; }

        [Column(TypeName = "money")]
        public decimal? ProdComisionVenta { get; set; }

        [Column(TypeName = "money")]
        public decimal? ProdComisionPago { get; set; }
    }
}
