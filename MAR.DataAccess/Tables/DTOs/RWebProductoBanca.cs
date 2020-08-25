namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RWebProductoBanca")]
    public partial class RWebProductoBanca
    {
        [Key]
        public int WebProductoBancaID { get; set; }

        public int WebProductoID { get; set; }

        public int BancaID { get; set; }
    }
}
