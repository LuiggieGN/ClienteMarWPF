
using System;
namespace MAR.DataAccess.Tables.ControlEfectivoDTOs
{
    public class RutaAsignacionDTO
    {
        public DateTime FechaCreacion { get; set; }
        public DateTime Dia { get; set; }
        public int RutaAsignacionID { get; set; }
        public string CodigoReferencia { get; set; }
        public bool AsignacionEstaActiva { get; set; }
        public string Gestor { get; set; }
        public int GestorUsuarioId { get; set; }
        public int GestorCajaId { get; set; }
        public decimal GestorBalanceMinimo { get; set; }
        public decimal GestorBalanceActual { get; set; }
        public int RutaId { get; set; }
        public string OrdenRecorrido { get; set; }
        public string Estado { get; set; }
        public int? UltimaLocalidadTerminalId { get; set; }
        public string UltimaLocalidadTerminalNombre { get; set; }
        public int RutaConfigId { get; set; }
        public string RutaConfigNombre { get; set; }

        public RutaRecorridoDTO RutaRecorridoDTO { get; set; }
    }
}
