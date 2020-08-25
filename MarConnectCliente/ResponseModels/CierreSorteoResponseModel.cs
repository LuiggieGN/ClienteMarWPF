using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarConnectCliente.ResponseModels
{
    public class CierreSorteoResponseModel : BaseResponseModel
    {
        public string NumeroAutentificacionCalculado { get; set; }
        public string NumeroAutentificacion { get; set; }
        public string Estado { get; set; }
        //public string CodigoOperacion { get; set; }
        public override string ToString()
        {
            return this.CierreSorteoAnon().ToString();
        }

        public object CierreSorteoAnon()
        {
            var ModelAnnon = new
            {
                NumeroAutentificacion = this.NumeroAutentificacion ?? "",
                NumeroAutentificacionCalculado = this.NumeroAutentificacionCalculado,
                Estado = this.Estado ?? "",
                CodigoRespuesta = this.CodigoRespuesta ?? "",
                MensajeRespuesta = this.MensajeRespuesta ?? "",
                Peticion = this.Peticion
            };

            return ModelAnnon;
        }
    }
}
