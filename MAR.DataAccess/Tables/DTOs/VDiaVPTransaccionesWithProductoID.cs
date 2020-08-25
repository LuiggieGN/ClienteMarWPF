namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VDiaVPTransaccionesWithProductoID")]
    public partial class VDiaVPTransaccionesWithProductoID
    {
        public DateTime? VenFecha { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RiferoID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BancaID { get; set; }

        public double? BanComision { get; set; }

        public double? SupComision { get; set; }

        public double? SupImpuesto { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SuplidorID { get; set; }

        public string SupNombre { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UsuarioID { get; set; }

        [StringLength(20)]
        public string Vendedor { get; set; }

        public int? CPagos { get; set; }

        [Column(TypeName = "money")]
        public decimal? VPagos { get; set; }

        [Column(TypeName = "money")]
        public decimal? VDescuentos { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductoID { get; set; }
    }
}
