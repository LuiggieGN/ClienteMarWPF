namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VP_ModeloVehiculo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ModeloID { get; set; }

        [Required]
        [StringLength(100)]
        public string Modelo { get; set; }

        public int MarcaID { get; set; }

        public virtual VP_MarcaVehiculo VP_MarcaVehiculo { get; set; }
    }
}
