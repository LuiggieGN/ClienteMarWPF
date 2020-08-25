namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DTicket
    {
        [Key]
        public int TicketID { get; set; }

        public int GrupoID { get; set; }

        public int LoteriaID { get; set; }

        public int RiferoID { get; set; }

        public int BancaID { get; set; }

        public int UsuarioID { get; set; }

        public DateTime TicFecha { get; set; }

        [Required]
        [StringLength(50)]
        public string TicCliente { get; set; }

        [Required]
        [StringLength(25)]
        public string TicCedula { get; set; }

        [StringLength(15)]
        public string TicNumero { get; set; }

        public bool TicNulo { get; set; }

        [Column(TypeName = "money")]
        public decimal TicCosto { get; set; }

        [Column(TypeName = "money")]
        public decimal TicPago { get; set; }

        [Column(TypeName = "numeric")]
        public decimal TicSolicitud { get; set; }

        public bool TicPagado { get; set; }

        public int PagoID { get; set; }
    }
}
