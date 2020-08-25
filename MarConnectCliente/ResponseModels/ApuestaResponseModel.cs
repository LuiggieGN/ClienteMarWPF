using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarConnectCliente.ResponseModels
{
    public class ApuestaResponseModel:BaseResponseModel
    {
        public string TipoAutentificacion { get; set; } //(Online - Offline)
        public string NumeroAutentificacion { get; set; } // naut
        public JugadorBalance JugadorBalance { get; set; }
        public List<int> SorteosNoDisponible { get; set; }
        public List<TicketResponseModel.TicketDetalle> TicketDetalle { get; set; }
        public override string ToString()
        {
            return this.ApuestaAnon().ToString();
        }

        public object ApuestaAnon()
        {

            var ModelAnnon = new
            {
                NumeroAutentificacion = this.NumeroAutentificacion ?? "",
                TipoAutentificacion = this.TipoAutentificacion??"",
                CodigoRespuesta = this.CodigoRespuesta??"",
                MensajeRespuesta = this.MensajeRespuesta??"",
                JugadorBalance =  this.JugadorBalance == null ? "": this.JugadorBalance.ToString(),
                SorteosNoDisponible = this.SorteosNoDisponible == null ? "[ ]" :  $"[{  string.Join(",",  SorteosNoDisponible) }]",
                Peticion = this.Peticion
            };

            return ModelAnnon;
        }
    }

    public class JugadorBalance
    {
        public decimal? BalanceInical { get; set; }
        public decimal? BalanceActual { get; set; }
        public decimal MontoDebitado { get; set; }

        public override string ToString()
        {
            return new
            {
                BalanceInical    = BalanceInical.HasValue ? BalanceInical.Value : 0,
                BalanceActual   = BalanceActual.HasValue ? BalanceActual.Value : 0,
                MontoDebitado = MontoDebitado

            }.ToString();
        }
    }


}
