namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HDestinos")]
    public partial class HDestino
    {
        [Key]
        public int DestinoID { get; set; }

        [Required]
        [StringLength(15)]
        public string DesIPAddress { get; set; }

        public DateTime DesInicio { get; set; }

        public DateTime DesAlive { get; set; }
    }
}
