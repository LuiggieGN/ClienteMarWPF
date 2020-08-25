using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarConnectCliente.ResponseModels
{
    public class VentaCompraMayoreoResponseModel : BaseResponseModel
    {
        public string TipoAutentificacion { get; set; } //(Online - Offline)
        public string NumeroAutentificacion { get; set; } // naut


        public object VentaAnonima()
        {

            var ModelAnnon = new
            {
                NumeroAutentificacion = this.NumeroAutentificacion,
                TipoAutentificacion = this.TipoAutentificacion,
                CodigoRespuesta = this.CodigoRespuesta,
                MensajeRespuesta = this.MensajeRespuesta,
                Peticion = this.Peticion
            };

            return ModelAnnon;
        }

        public object CompraAnonima()
        {

            var ModelAnnon = new
            {
                TipoAutentificacion = this.TipoAutentificacion,
                CodigoRespuesta = this.CodigoRespuesta,
                MensajeRespuesta = this.MensajeRespuesta,
                Peticion = this.Peticion
            };

            return ModelAnnon;
        }
    }
}
