﻿
namespace ClienteMarWPFWin7.Domain.Models.Dtos.EfectivoDtos
{
    public class BancaControlEfectivoTransaccionDTO
    {
        public int LogicaKey { get; set; }
        public int TipoMovimientoSistemaID { get; set; }
        public string IngresoOEgreso { get; set; }
        public string Tipo { get; set; }
        public string TipoNombre { get; set; }
        public decimal MontoAcumulado { get; set; }
        public bool EsTipoSistema { get; set; }
    }
}
