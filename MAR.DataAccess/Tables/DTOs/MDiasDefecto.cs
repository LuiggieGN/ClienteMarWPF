namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MDiasDefecto")]
    public partial class MDiasDefecto
    {
        [Key]
        public int DiaDefectoID { get; set; }

        public int LoteriaID { get; set; }

        public int GrupoID { get; set; }

        public int DDeDia { get; set; }

        public int DDeHoraInicio { get; set; }

        public int DDeHoraFin { get; set; }

        [Column(TypeName = "money")]
        public decimal DDeCostoQ { get; set; }

        [Column(TypeName = "money")]
        public decimal DDeCostoP { get; set; }

        [Column(TypeName = "money")]
        public decimal DDeCostoT { get; set; }

        [Column(TypeName = "money")]
        public decimal DDePagoQ1 { get; set; }

        [Column(TypeName = "money")]
        public decimal DDePagoQ2 { get; set; }

        [Column(TypeName = "money")]
        public decimal DDePagoQ3 { get; set; }

        [Column(TypeName = "money")]
        public decimal DDePagoP1 { get; set; }

        [Column(TypeName = "money")]
        public decimal DDePagoP2 { get; set; }

        [Column(TypeName = "money")]
        public decimal DDePagoP3 { get; set; }

        [Column(TypeName = "money")]
        public decimal DDePagoT1 { get; set; }

        [Column(TypeName = "money")]
        public decimal DDePagoT2 { get; set; }

        [Column(TypeName = "money")]
        public decimal DDePagoT3 { get; set; }

        public double? DDeMaxPale { get; set; }

        public double? DDeMaxQuiniela { get; set; }

        public double? DDeMaxTriple { get; set; }

        public int EsquemaID { get; set; }
    }
}
