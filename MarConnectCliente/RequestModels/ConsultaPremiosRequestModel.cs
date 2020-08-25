using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarConnectCliente.RequestModels
{
    public  class ConsultaPremiosRequestModel : BaseRequestModel
    {

        public override string ToString()
        {
            return new
            {
                CodigoOperacion = this.CodigoOperacion,
                TipoOperacion = this.TipoOperacion,
                EstablecimientoID = this.EstablecimientoID,
                FechaHoraSolicitud = this.FechaHoraSolicitud
            }.ToString();
        }

    }
}
