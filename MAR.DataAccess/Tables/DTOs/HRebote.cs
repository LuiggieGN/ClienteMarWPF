namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class HRebote
    {
        [Key]
        public int ReboteID { get; set; }

        public DateTime RebFecha { get; set; }

        public int ConsorcioID { get; set; }

        public int LoteriaID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal RebTotal { get; set; }

        [Column(TypeName = "numeric")]
        public decimal RebComision { get; set; }

        [Column(TypeName = "numeric")]
        public decimal RebNeto { get; set; }

        [Required]
        [StringLength(15)]
        public string RebTicketAqui { get; set; }

        [Required]
        [StringLength(15)]
        public string RebTicketAlla { get; set; }

        [Column(TypeName = "numeric")]
        public decimal RebSaco { get; set; }

        [Column(TypeName = "numeric")]
        public decimal RebBalance { get; set; }

        public bool RebNulo { get; set; }
    }
}
