namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RCuentasBanca
    {
        [Key]
        public int CuentaBancaID { get; set; }

        public int CuentaID { get; set; }

        public int BancaID { get; set; }

        public DateTime CBaDesde { get; set; }
    }
}
