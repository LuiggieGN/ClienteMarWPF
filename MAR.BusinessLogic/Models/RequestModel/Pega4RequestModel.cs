using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAR.BusinessLogic.Models.RequestModel
{
   public class Pega4RequestModel
    {
        public class CLReciboRequestModel
        {
            public string ReciboID { get; set; }
            public string Referencia { get; set; }
            public string Serie { get; set; }
            public int SolicitudID { get; set; }
            public DateTime Fecha { get; set; }
            public int BancaID { get; set; }
            public int UsuarioID { get; set; }
            public int ClienteID { get; set; }
            public decimal Ingresos { get; set; }
            public decimal Impuestos { get; set; }
            public decimal Descuentos { get; set; }
            public int MonedaID { get; set; }
        }

        public class RFTransaccionRequestModel
        {
            public ICollection<RFTransacciones> Transacciones { get; set; }
            public class RFTransacciones
            {
                public DateTime FechaIngreso { get; set; }
                public string Referencia { get; set; }
                public string Notas { get; set; }
                public int ReciboID { get; set; }
                public int SolicitudID { get; set; }
                public int SorteoDiaID { get; set; }
                public int EsquemaPagoID { get; set; }
                public decimal Ingresos { get; set; }
                public string Estado { get; set; }
                public string TipoSorteo { get; set; }
                public ICollection<RFTransaccionJugadaRequestModel> TransaccionJugadas { get; set; }

                public class RFTransaccionJugadaRequestModel
                {
                    public string Numeros { get; set; }
                    public int Orden { get; set; }
                    public decimal Aposto { get; set; }
                    public decimal Pago { get; set; }
                    public string Opciones { get; set; }
                    public int SorteoTipoJugadaID { get; set; }

                }
            }
        }
    }
}
