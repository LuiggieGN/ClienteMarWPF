namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class HMensaje
    {
        public int HMensajeID { get; set; }

        public int MensajeID { get; set; }

        [Required]
        [StringLength(12)]
        public string MenTipo { get; set; }

        [Required]
        [StringLength(50)]
        public string MenAsunto { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string MenContenido { get; set; }

        public DateTime MenFecha { get; set; }

        [Required]
        [StringLength(200)]
        public string MenDestino { get; set; }

        [Required]
        [StringLength(1)]
        public string MenDireccion { get; set; }

        public int BancaID { get; set; }

        [Required]
        [StringLength(50)]
        public string MenOrigen { get; set; }

        public bool MenLeido { get; set; }
    }
}
