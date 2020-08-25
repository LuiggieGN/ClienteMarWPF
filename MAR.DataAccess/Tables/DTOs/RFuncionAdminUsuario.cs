namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RFuncionAdminUsuario")]
    public partial class RFuncionAdminUsuario
    {
        [Key]
        public int FuncionAdminUsuarioID { get; set; }

        public int FuncionAdminID { get; set; }

        public int UsuarioID { get; set; }
    }
}
