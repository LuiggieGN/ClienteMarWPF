using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarConnectCliente.ResponseModels
{
    public class CompraFondosResponseModel : BaseResponseModel
    {

        public string TipoAutentificacion { get; set; } //(Online - Offline)
        public string NumeroAutentificacion { get; set; } // naut
        public string Estado { get; set; }
        public decimal BalanceActual { get; set; }

        public override string ToString()
        {
            return new
            {
                BalanceActual = BalanceActual,
                CodigoRespuesta = CodigoRespuesta,
                Estado = Estado,
                FechaHoraRespuesta = FechaHoraRespuesta,
                MensajeRespuesta = MensajeRespuesta,
                NumeroAutentificacion = NumeroAutentificacion,
                Peticion = Peticion,
                TipoAutentificacion = TipoAutentificacion
            }.ToString();
        }

    }
}
