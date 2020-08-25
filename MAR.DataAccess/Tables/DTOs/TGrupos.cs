namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TGrupos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TGrupos()
        {
            DControles = new HashSet<DControle>();
            HControles = new HashSet<HControle>();
            HEstatusDias = new HashSet<HEstatusDia>();
            HTickets = new HashSet<HTicket>();
            MPrecios = new HashSet<MPrecio>();
            MRiferos = new HashSet<MRifero>();
        }

        [Key]
        public int GrupoID { get; set; }

        [Required]
        [StringLength(50)]
        public string GruNombre { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string GruContacto { get; set; }

        [Required]
        [StringLength(20)]
        public string GruTelefono { get; set; }

        [Required]
        [StringLength(21)]
        public string GruCelular { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string GruComentario { get; set; }

        public bool GruActivo { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string GruClientFooter { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string GruPrintHeader { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string GruPrintFooter { get; set; }

        public bool GruLimiteGlo { get; set; }

        public bool GruLimiteNum { get; set; }

        public bool GruLimiteNumRifero { get; set; }

        public bool GruLimiteGloBanca { get; set; }

        public int GruMinutosAnula { get; set; }

        public int GruDelayCierre { get; set; }

        public int GruKeepAlive { get; set; }

        public int GruMantenimiento { get; set; }

        public bool GruLimiteTicBanca { get; set; }

        public bool GruDosLoterias { get; set; }

        public bool GruLimiteBanDinero { get; set; }

        public bool? GrupEnEscrutineo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DControle> DControles { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HControle> HControles { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HEstatusDia> HEstatusDias { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HTicket> HTickets { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MPrecio> MPrecios { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MRifero> MRiferos { get; set; }
    }
}
