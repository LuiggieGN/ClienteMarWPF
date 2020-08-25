namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RF_Sorteo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RF_Sorteo()
        {
            RF_SorteoCampo = new HashSet<RF_SorteoCampo>();
        }

        [Key]
        public int SorteoID { get; set; }

        [Required]
        [StringLength(250)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(250)]
        public string Referencia { get; set; }

        public int LoteriaID { get; set; }

        public int SorteoTipoID { get; set; }

        public int Activo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RF_SorteoCampo> RF_SorteoCampo { get; set; }
    }
}
