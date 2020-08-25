namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PVPine
    {
        [Key]
        [Column(Order = 0)]
        public int PinID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SuplidorID { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CuentaID { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductoID { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(50)]
        public string PinSerial { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(20)]
        public string PinNumero { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(10)]
        public string PinReferencia { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(2)]
        public string PinCodigo { get; set; }

        [Key]
        [Column(Order = 8)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PinNulo { get; set; }

        [Key]
        [Column(Order = 9)]
        [StringLength(500)]
        public string PinMensaje { get; set; }

        [Key]
        [Column(Order = 10, TypeName = "numeric")]
        public decimal PinSecuencia { get; set; }

        [Key]
        [Column(Order = 11)]
        [StringLength(1)]
        public string PinFlag { get; set; }

        [Key]
        [Column(Order = 12)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BancaID { get; set; }

        [Key]
        [Column(Order = 13)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RiferoID { get; set; }

        [Key]
        [Column(Order = 14)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int GrupoID { get; set; }

        [Key]
        [Column(Order = 15)]
        public DateTime PinFecha { get; set; }

        [Key]
        [Column(Order = 16)]
        [StringLength(15)]
        public string PinIPAddr { get; set; }

        [Key]
        [Column(Order = 17, TypeName = "numeric")]
        public decimal PinCosto { get; set; }

        [Key]
        [Column(Order = 18)]
        public double PinImpuesto { get; set; }

        [Key]
        [Column(Order = 19)]
        public double PinComision { get; set; }

        [Key]
        [Column(Order = 20)]
        [StringLength(124)]
        public string ProNombre { get; set; }

        [Key]
        [Column(Order = 21)]
        [StringLength(50)]
        public string SupNombre { get; set; }

        [Key]
        [Column(Order = 22)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProPrecio { get; set; }
    }
}
