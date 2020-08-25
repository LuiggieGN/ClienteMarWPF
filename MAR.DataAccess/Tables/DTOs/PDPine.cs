namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PDPine
    {
        [Key]
        public int PinID { get; set; }

        public int SuplidorID { get; set; }

        public int CuentaID { get; set; }

        public int ProductoID { get; set; }

        [Required]
        [StringLength(50)]
        public string PinSerial { get; set; }

        [Required]
        [StringLength(20)]
        public string PinNumero { get; set; }

        [Required]
        [StringLength(10)]
        public string PinReferencia { get; set; }

        [Required]
        [StringLength(2)]
        public string PinCodigo { get; set; }

        public int PinNulo { get; set; }

        [Required]
        [StringLength(500)]
        public string PinMensaje { get; set; }

        [Column(TypeName = "numeric")]
        public decimal PinSecuencia { get; set; }

        [Required]
        [StringLength(1)]
        public string PinFlag { get; set; }

        public int BancaID { get; set; }

        public int RiferoID { get; set; }

        public int GrupoID { get; set; }

        public DateTime PinFecha { get; set; }

        [Required]
        [StringLength(15)]
        public string PinIPAddr { get; set; }

        [Column(TypeName = "numeric")]
        public decimal PinCosto { get; set; }

        public double PinImpuesto { get; set; }

        public double PinComision { get; set; }
    }
}
