namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HTicketsLocal")]
    public partial class HTicketsLocal
    {
        [Key]
        public int TicketLocalID { get; set; }

        public int GrupoID { get; set; }

        public int LoteriaID { get; set; }

        public int RiferoID { get; set; }

        public int BancaID { get; set; }

        public int UsuarioID { get; set; }

        public DateTime TLoFecha { get; set; }

        [Required]
        [StringLength(50)]
        public string TLoCliente { get; set; }

        [Required]
        [StringLength(25)]
        public string TLoCedula { get; set; }

        [StringLength(25)]
        public string TLoNumero { get; set; }

        public bool TLoNulo { get; set; }

        [Column(TypeName = "money")]
        public decimal TLoCosto { get; set; }

        [Column(TypeName = "money")]
        public decimal TLoPago { get; set; }

        [Column(TypeName = "numeric")]
        public decimal TLoSolicitud { get; set; }
    }
}
