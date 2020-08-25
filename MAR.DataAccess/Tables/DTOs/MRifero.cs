namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MRiferos")]
    public partial class MRifero
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MRifero()
        {
            MBancas = new HashSet<MBanca>();
        }

        [Key]
        public int RiferoID { get; set; }

        [Required]
        [StringLength(50)]
        public string RifNombre { get; set; }

        [Required]
        [StringLength(50)]
        public string RifContacto { get; set; }

        [Required]
        [StringLength(20)]
        public string RifTelefono { get; set; }

        [Required]
        [StringLength(20)]
        public string RifCelular { get; set; }

        [Required]
        [StringLength(20)]
        public string RifCedula { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string RifComentario { get; set; }

        public int GrupoID { get; set; }

        public bool RifActivo { get; set; }

        public int? ZonaID { get; set; }

        public double RifDescuento { get; set; }

        public double RifCaida { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MBanca> MBancas { get; set; }

        public virtual TGrupos TGrupos { get; set; }
    }
}
