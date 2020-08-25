using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarConnectCliente.RequestModels
{
    public  class ConsultaSacoRequestModel : BaseRequestModel
    {
        public string FechaDesde { get; set; }
        public string FechaHasta { get; set; }
        public int TerminalID { get; set; }

        public override string ToString()
        {
            return new
            {
                FechaDesde = this.FechaDesde,
                FechaHasta = this.FechaHasta,
                CodigoOperacion = this.CodigoOperacion,
                TipoOperacion = this.TipoOperacion,
                EstablecimientoID = this.EstablecimientoID,
                FechaHoraSolicitud = this.FechaHoraSolicitud
            }.ToString();
        }

    }
}
