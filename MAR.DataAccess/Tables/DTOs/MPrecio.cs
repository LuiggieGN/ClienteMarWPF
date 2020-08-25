namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MPrecio
    {
        [Key]
        public int PrecioID { get; set; }

        public int LoteriaID { get; set; }

        public int GrupoID { get; set; }

        public double PreCostoQ { get; set; }

        public double PreCostoP { get; set; }

        public double PreCostoT { get; set; }

        public double PrePagoQ1 { get; set; }

        public double PrePagoQ2 { get; set; }

        public double PrePagoQ3 { get; set; }

        public double PrePagoP1 { get; set; }

        public double PrePagoP2 { get; set; }

        public double PrePagoP3 { get; set; }

        public double PrePagoT1 { get; set; }

        public double PrePagoT2 { get; set; }

        public double PrePagoT3 { get; set; }

        public int EsquemaID { get; set; }

        public virtual TGrupos TGrupos { get; set; }
    }
}
