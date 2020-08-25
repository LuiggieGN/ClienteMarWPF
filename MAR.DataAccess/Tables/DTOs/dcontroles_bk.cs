namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class dcontroles_bk
    {
        [Key]
        [Column(Order = 0)]
        public int ControlID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int GrupoID { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(30)]
        public string ConNombre { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ConLimite { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ConPorUserID { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(7)]
        public string ConColor { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(1)]
        public string ConQP { get; set; }
    }
}
