using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarConnectCliente.ResponseModels
{
    public class InicioDiaReponseModel:BaseResponseModel
    {
        
        public string CodigoValidacionOffline { get; set; }
        public string Estado { get; set; }
        public string CodigoOperacion { get; set; }

        //public override string ToString()
        //{
        //    var _r = new
        //    {
        //        CodigoValidacionOffline = CodigoValidacionOffline ?? "",
        //        Estado = Estado ?? "",
        //        CodigoOperacion = CodigoOperacion ?? "",
        //        TipoOperacion = "Inicio",
        //        MensajeRespuesta = ""

        //    };
        //    return _r.ToString();
        //}

        public override string ToString()
        {
            return this.InicioAnon().ToString();
        }

        public object InicioAnon()
        {
            var ModelAnnon = new
            {
                CodigoValidacionOffline = this.CodigoValidacionOffline ?? "",
                Estado = this.Estado ?? "",
                CodigoRespuesta = this.CodigoRespuesta ?? "",
                MensajeRespuesta = this.MensajeRespuesta ?? "",
                TipoOperacion = "Inicio",
                Peticion = this.Peticion
            };
            return ModelAnnon;
        }
    }
}
