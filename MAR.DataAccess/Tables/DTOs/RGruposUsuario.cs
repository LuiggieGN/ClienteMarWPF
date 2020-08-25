namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RGruposUsuario
    {
        [Key]
        public int GrupoUsuarioID { get; set; }

        public int UsuarioID { get; set; }

        public int GrupoID { get; set; }
    }
}
