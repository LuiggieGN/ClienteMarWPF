using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarConnectCliente.ResponseModels
{
    public class BaseResponseModel
    {
        public string FechaHoraRespuesta { get; set; }
        public string CodigoRespuesta { get; set; }
        public string MensajeRespuesta { get; set; }
        public object Peticion { get; set; }
        public string Cod { get; set; }
        public string Msg { get; set; }
    }
}
