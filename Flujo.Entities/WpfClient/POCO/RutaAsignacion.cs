using System;

namespace Flujo.Entities.WpfClient.POCO
{
    public class RutaAsignacion
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

    }


    public class RutaRecorrido 
    {
        public TerminalRecord[] Terminales { get; set; }
        public decimal GestorBalanceAlAsignarRuta { get; set; }
        public decimal GestorMontoEntregado { get; set; }
    }

    public class TerminalRecord
    {
        public int BancaID { get; set; }
        public string Terminal { get; set; }
        public int Orden { get; set; }
        public string Direccion { get; set; }
        public bool FueRecorrida { get; set; }
        public bool IncluirEnRecorrido { get; set; }
        public int DepositarORetirar { get; set; }
        public decimal MontoRuta { get; set; }
        public decimal BalanceAlCuadreGestor { get; set; }
    }


}
