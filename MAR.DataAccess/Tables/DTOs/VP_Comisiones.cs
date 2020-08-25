namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VP_Comisiones
    {
        [Key]
        public int ComisionID { get; set; }

        public int BancaID { get; set; }

        public int VP_ProductoID { get; set; }

        public double Porciento { get; set; }

        public virtual MBanca MBanca { get; set; }

        public virtual VP_Producto VP_Producto { get; set; }
    }
}
