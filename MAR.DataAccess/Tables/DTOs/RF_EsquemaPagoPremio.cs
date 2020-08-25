namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RF_EsquemaPagoPremio
    {
        [Key]
        public int EsquemaPagoPremioID { get; set; }

        public int EsquemaPagoID { get; set; }

        public int? SorteoID { get; set; }

        public int SorteoTipoPremioID { get; set; }

        [Column(TypeName = "money")]
        public decimal Paga { get; set; }

        [StringLength(2)]
        public string DiaSemana { get; set; }

        public bool Activo { get; set; }
    }
}
