namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CL_Cliente
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CL_Cliente()
        {
            CL_ClienteCampo = new HashSet<CL_ClienteCampo>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ClienteID { get; set; }

        [StringLength(250)]
        public string Nombres { get; set; }

        [StringLength(250)]
        public string Apellidos { get; set; }

        [Required]
        [StringLength(100)]
        public string Documento { get; set; }

        public int TipoDocumentoID { get; set; }

        public DateTime FechaCreado { get; set; }

        public DateTime? UltimaAccion { get; set; }

        public bool Activo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CL_ClienteCampo> CL_ClienteCampo { get; set; }
    }
}
