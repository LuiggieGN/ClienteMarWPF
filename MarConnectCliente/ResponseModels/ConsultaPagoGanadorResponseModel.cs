using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarConnectCliente.ResponseModels
{
    public partial class ConsultaPagoGanadorResponseModel : BaseResponseModel
    {
        public ConsultaPagoGanadorResponseModel() { }
        public ConsultaPagoGanadorResponseModel(ConsultaPagoGanadorResponseModel _response) : base()
        {
            base.FechaHoraRespuesta = _response.FechaHoraRespuesta;
            base.CodigoRespuesta = _response.CodigoRespuesta;
            base.MensajeRespuesta = _response.MensajeRespuesta;
            base.Peticion = _response.Peticion;
            this.Saco = _response.Saco;

        }
    }
    public partial class ConsultaPagoGanadorResponseModel : BaseResponseModel
    {
        public double Saco { get; set; }
        public int TicketEstado { get; set; }
        public List<TicketResponseModel.TicketDetalle> TicketDetalle { get; set; }
    }
}