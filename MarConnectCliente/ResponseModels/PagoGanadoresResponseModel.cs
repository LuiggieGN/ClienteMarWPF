using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarConnectCliente.ResponseModels
{
    public class PagoGanadoresResponseModel:BaseResponseModel
    {
        public double MontoISR { get; set; }
        public string TipoAutentificacion { get; set; } //(Online - Offline)
        public string NumeroAutentificacion { get; set; } // naut
      
    }
}
