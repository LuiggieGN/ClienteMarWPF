using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarConnectCliente.ResponseModels
{
    public class CierreDiaResponseModel : BaseResponseModel
    {
        public string NumeroAutentificacion { get; set; }
        public string Estado { get; set; }
        //public string CodigoOperacion { get; set; }
        public override string ToString()
        {
            return this.CierreDiaAnon().ToString();
        }

        public object CierreDiaAnon()
        {
            var ModelAnnon = new
            {
                NumeroAutentificacion = this.NumeroAutentificacion ?? "",
                Estado = this.Estado ?? "",
                CodigoRespuesta = this.CodigoRespuesta ?? "",
                MensajeRespuesta = this.MensajeRespuesta ?? "",
                Peticion = this.Peticion
            };

            return ModelAnnon;
        }
    }
}
