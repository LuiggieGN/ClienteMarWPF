namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MBanca
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MBanca()
        {
            VP_Comisiones = new HashSet<VP_Comisiones>();
        }

        [Key]
        public int BancaID { get; set; }

        [Required]
        [StringLength(50)]
        public string BanNombre { get; set; }

        [Required]
        [StringLength(50)]
        public string BanContacto { get; set; }

        [Required]
        [StringLength(50)]
        public string BanDireccion { get; set; }

        [Required]
        [StringLength(15)]
        public string BanDireccionIP { get; set; }

        [Required]
        [StringLength(20)]
        public string BanTelefono { get; set; }

        [Required]
        [StringLength(20)]
        public string BanNumeroLinea { get; set; }

        public int RiferoID { get; set; }

        public bool BanActivo { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string BanComentario { get; set; }

        public bool BanValidIP { get; set; }

        public int BanSesionActual { get; set; }

        [Required]
        [StringLength(25)]
        public string BanDireccionActual { get; set; }

        public int BanUsuarioActual { get; set; }

        public int BanRePrintTicketID { get; set; }

        public int BanUltimoTicket { get; set; }

        [Required]
        [StringLength(20)]
        public string BanVersion { get; set; }

        public double BanMaxQuiniela { get; set; }

        public double BanMaxPale { get; set; }

        public double BanMaxTriple { get; set; }

        public double BanComisionQ { get; set; }

        public double BanComisionP { get; set; }

        public double BanComisionT { get; set; }

        public int EsquemaID { get; set; }

        public DateTime BanAlive { get; set; }

        public DateTime BanFirstContact { get; set; }

        public DateTime BanLastContact { get; set; }

        public bool BanAnula { get; set; }

        public bool BanVLocal { get; set; }

        public bool BanGanadores { get; set; }

        public double BanMaxQuinielaLoc { get; set; }

        public double BanMaxPaleLoc { get; set; }

        public double BanMaxTripleLoc { get; set; }

        public bool BanTarjeta { get; set; }

        public double BanComisionTarj { get; set; }

        public int BanTerminalTarj { get; set; }

        [Required]
        [StringLength(8)]
        public string BanSerieTarj { get; set; }

        public bool BanRenta { get; set; }

        public DateTime BanRegistra { get; set; }

        [Required]
        [StringLength(100)]
        public string BanRemoteCMD { get; set; }

        public double BanMaxSupeLoc { get; set; }

        public double BanMaxQuinielaTic { get; set; }

        public double BanMaxPaleTic { get; set; }

        public double BanMaxTripleTic { get; set; }

        public int BanSesionWeb { get; set; }

        [Required]
        [StringLength(25)]
        public string BanDireccionWeb { get; set; }

        public int BanUsuarioWeb { get; set; }

        public double BanMaxSupe { get; set; }

        public bool BanImpuesto { get; set; }

        public bool BanPrintRecarga { get; set; }

        public bool BanPrintReportes { get; set; }

        public bool? BanBillete { get; set; }

        public bool? BanServicios { get; set; }

        public virtual MRifero MRifero { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VP_Comisiones> VP_Comisiones { get; set; }
    }
}
