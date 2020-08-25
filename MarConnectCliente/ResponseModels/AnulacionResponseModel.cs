using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarConnectCliente.ResponseModels
{
    public partial class AnulacionResponseModel : BaseResponseModel
    {
        public AnulacionResponseModel() { }
        public AnulacionResponseModel(AnulacionResponseModel _response) : base()
        {
            base.FechaHoraRespuesta = _response.FechaHoraRespuesta;
            base.CodigoRespuesta = _response.CodigoRespuesta;
            base.MensajeRespuesta = _response.MensajeRespuesta;
            base.Peticion = _response.Peticion;
            this.TipoAutentificacion = _response.TipoAutentificacion;
            this.NumeroAutentificacion = _response.NumeroAutentificacion;

        }


        public override string ToString()
        {
            var ModelAnnon = new
            {
                NumeroAutentificacion = this.NumeroAutentificacion ?? "",
                TipoAutentificacion = this.TipoAutentificacion ?? "",
                CodigoRespuesta = this.CodigoRespuesta ?? "",
                MensajeRespuesta = this.MensajeRespuesta ?? "",
                Peticion = this.Peticion
            };

            return ModelAnnon.ToString();
        }
    }


    public partial class AnulacionResponseModel : BaseResponseModel
    {
        public string NumeroAutentificacion { get; set; } //(Naut Calculado)
        public string TipoAutentificacion { get; set; } //(Online / Offline)

    }

    public partial class AnulacionResponseWithJugador : AnulacionResponseModel
    {
        public AnulacionResponseWithJugador(AnulacionResponseModel _response, JugadorData _jugador) : base(_response)
        {
            this.Jugador = _jugador;
        }
    }

    public partial class AnulacionResponseWithJugador : AnulacionResponseModel
    {
        public JugadorData Jugador { get; set; }
    }

    public class JugadorData
    {
        public decimal BalanceActual { get; set; }
    }

}