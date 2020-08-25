namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HPagos")]
    public partial class HPago
    {
        [Key]
        public int PagoID { get; set; }

        public DateTime PagFecha { get; set; }

        public int BancaID { get; set; }

        [Required]
        [StringLength(50)]
        public string BanContacto { get; set; }

        [Column(TypeName = "money")]
        public decimal PagMonto { get; set; }

        [Required]
        [StringLength(20)]
        public string PagUsuario { get; set; }

        [Required]
        [StringLength(15)]
        public string TicNumero { get; set; }

        public DateTime EdiFecha { get; set; }

        public int LoteriaID { get; set; }

        [Column(TypeName = "date")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? PagDia { get; set; }
    }
}
