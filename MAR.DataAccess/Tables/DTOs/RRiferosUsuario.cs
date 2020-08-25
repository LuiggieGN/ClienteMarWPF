namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RRiferosUsuario
    {
        [Key]
        public int BancaUsuarioID { get; set; }

        public int RiferoID { get; set; }

        public int UsuarioID { get; set; }
    }
}
