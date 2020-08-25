namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RF_TransaccionJugada
    {
        [Key]
        public int TransaccionJugadaID { get; set; }

        public int TransaccionID { get; set; }

        public int SorteoTipoJugadaID { get; set; }

        [Required]
        public string Numeros { get; set; }

        public int Orden { get; set; }

        [Column(TypeName = "money")]
        public decimal Aposto { get; set; }

        [Column(TypeName = "money")]
        public decimal? Pago { get; set; }

        public string Opciones { get; set; }
    }
}
