using System;
 
namespace MAR.DataAccess.Tables.ControlEfectivoDTOs
{
    public class CajaDTO
    {
        public int CajaID { get; set; }
        public int TipoCajaID { get; set; }
        public int? ZonaID { get; set; }
        public int? UsuarioID { get; set; }
        public int? BancaID { get; set; }
        public string Ubicacion { get; set; }
        public decimal BalanceActual { get; set; }
        public decimal BalanceMinimo { get; set; }
        public DateTime FechaBalance { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Activa { get; set; }
        public string CajaDescripcion { get; set; }
        public bool Disponible { get; set; }
    }
}
