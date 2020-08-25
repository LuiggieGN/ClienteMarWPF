namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SFuncionAdmin")]
    public partial class SFuncionAdmin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FuncionAdminID { get; set; }

        public int? ParentFuncionAdminID { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        public bool Disponible { get; set; }
    }
}
