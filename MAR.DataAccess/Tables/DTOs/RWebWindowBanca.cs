namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RWebWindowBanca")]
    public partial class RWebWindowBanca
    {
        [Key]
        public int WebWindowBancaID { get; set; }

        public int WebWindowID { get; set; }

        public int BancaID { get; set; }
    }
}
