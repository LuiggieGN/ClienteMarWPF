namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DBilleteDetalle")]
    public partial class DBilleteDetalle
    {
        [Key]
        public int BilleteDetalleID { get; set; }

        public int TicketDetalleID { get; set; }

        [Required]
        [StringLength(50)]
        public string Serial { get; set; }

        public DateTime HoraRequest { get; set; }

        public DateTime HoraResponse { get; set; }

        public int NumeroSorteo { get; set; }

        public virtual DTicketDetalle DTicketDetalle { get; set; }
    }
}
