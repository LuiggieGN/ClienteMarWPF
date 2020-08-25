using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Flujo.Entities.WpfClient.Enums;

namespace Flujo.Entities.WpfClient.POCO
{
    public class PrintDocumentTransaccionConCuadre
    {
        public string BanContacto { get; set; }
        public string BanDireccion { get; set; }
        public string CuadreTransaccion { get; set; }   //CuadreTransaccion {BancaId}-{Referencia}
        public string FechaTransaccion { get; set; }
        public string BalanceDeCajaSegunSistema { get; set; }
        public string EfectivoContadoEnCaja { get; set; }
        public FaltanteSobrante_Enum FaltanteSobrante { get; set; }
        public decimal AUX_MontoFaltanteOMontoSobrante { get; set; }
        public string  MontoFaltanteOMontoSobrante { get; set; }
        public string  Responsable { get; set; }
        public string  BanMontoRetiradoODepositado { get; set; }

        public string  BalanceFinalCaja { get; set; } 
        public string  RecibidoPor { get; set; }
        public bool    EsUnDeposito { get; set; } 
    }
}
