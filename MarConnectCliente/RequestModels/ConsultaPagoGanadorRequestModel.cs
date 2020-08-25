using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarConnectCliente.RequestModels
{
    public class ConsultaPagoGanadorRequestModel : BaseRequestModel
    {
        public string NoTicket { get; set; }
        public override string ToString()
        {
            return new
            {
                NoTicket = this.NoTicket,
                CodigoOperacion = this.CodigoOperacion,
                TipoOperacion = this.TipoOperacion,
                EstablecimientoID = this.EstablecimientoID,
                FechaHoraSolicitud = this.FechaHoraSolicitud
            }.ToString();
        }
    }
}
