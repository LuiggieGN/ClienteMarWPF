namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MUsuario
    {
        [Key]
        public int UsuarioID { get; set; }

        [Required]
        [StringLength(50)]
        public string UsuNombre { get; set; }

        [Required]
        [StringLength(50)]
        public string UsuApellido { get; set; }

        [Required]
        [StringLength(25)]
        public string UsuCedula { get; set; }

        public DateTime UsuFechaNac { get; set; }

        [Required]
        [StringLength(20)]
        public string UsuUserName { get; set; }

        [Required]
        [StringLength(50)]
        public string UsuClave { get; set; }

        public DateTime UsuVenceClave { get; set; }

        public bool UsuActivo { get; set; }

        public int UsuNivel { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string UsuComentario { get; set; }
    }
}
