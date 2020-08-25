namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RF_SorteoTipoJugada
    {
        [Key]
        public int SorteoTipoJugadaID { get; set; }

        public int SorteoTipoID { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        [StringLength(20)]
        public string Referencia { get; set; }

        public int CamposNumero { get; set; }

        public int NumeroMinimo { get; set; }

        public int NumeroMaximo { get; set; }

        public bool Activo { get; set; }

        public string Opciones { get; set; }

        public string Instrucciones { get; set; }

        public virtual RF_SorteoTipo RF_SorteoTipo { get; set; }
    }
}
