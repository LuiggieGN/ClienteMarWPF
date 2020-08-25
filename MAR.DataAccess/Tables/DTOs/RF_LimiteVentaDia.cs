namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RF_LimiteVentaDia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LimiteVentaDiaID { get; set; }

        [Column(TypeName = "date")]
        public DateTime Dia { get; set; }

        public int SorteoID { get; set; }

        public int BancaID { get; set; }

        public int RiferoID { get; set; }

        public int ZonaID { get; set; }

        public int TipoJugadaID { get; set; }

        [Column(TypeName = "money")]
        public decimal Vendido { get; set; }

        [Required]
        public string Numeros { get; set; }
    }
}
