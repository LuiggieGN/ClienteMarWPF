
using System;

namespace ClienteMarWPF.Domain.Models.Dtos.EfectivoDtos
{
    public class CuadreDTO
    {
        public int CuadreID { get; set; }
        public string Tipo { get; set; }
        public decimal? BalanceAnterior { get; set; }
        public int CajaID { get; set; }
        public string UsuarioCaja { get; set; }
        public int CajaOrigenID { get; set; }
        public int UsuarioOrigenID { get; set; }
        public int? CuadreAnteriorID { get; set; }
        public decimal BalanceMinimo { get; set; }
        public decimal MontoPorPagar { get; set; }
        public decimal MontoReal { get; set; }
        public decimal MontoContado { get; set; }
        public decimal MontoFaltante { get; set; }
        public decimal? MontoRetirado { get; set; }
        public decimal? MontoDepositado { get; set; }
        public decimal Balance { get; set; }
        public DateTime Fecha { get; set; }
        public decimal AuxMontoAFavor { get; set; }
        public string AuxMensajeroNombre { get; set; }

    }
}
