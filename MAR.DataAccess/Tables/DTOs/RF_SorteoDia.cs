namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RF_SorteoDia
    {
        [Key]
        public int SorteoDiaID { get; set; }

        public int SorteoID { get; set; }

        [Column(TypeName = "date")]
        public DateTime Dia { get; set; }

        public DateTime HoraInicioVentas { get; set; }

        public DateTime HoraCierreVentas { get; set; }

        public bool VentasCerradas { get; set; }

        public string Premios { get; set; }

        public DateTime? HoraPremios { get; set; }

        public bool DiaCerrado { get; set; }

        public DateTime? HoraCierreDia { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; }

        public string Referencia { get; set; }

        public bool PuedeReportar { get; set; }
    }
}
